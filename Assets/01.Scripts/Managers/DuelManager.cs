using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

using Photon.Realtime;

using NaughtyAttributes;
using Penwyn.Tools;

namespace Penwyn.Game
{
    public class DuelManager : SingletonMonoBehaviour<DuelManager>
    {
        public string BoardViewPrefabPath;
        public Transform BoardPosition;
        public EndGameUI EndGameUIPrefab;

        [Header("Decks")]
        public DeckManager MasterDMPrefab;
        public DeckManager GuestDMPrefab;
        [Header("Duel Settings")]
        public MatchSettings DuelSettings;

        private BoardView _boardView;
        private PhotonView photonView;


        private DeckManager _masterDM;
        private DeckManager _guestDM;


        private StateMachine<Phase> _phaseMachine;
        private Faction _currentFactionTurn = Faction.WHITE;
        private IntValue _currentTurnCount = new IntValue();
        private EndGameUI _endGameUI;



        //In-game params
        private bool _isGuestReady = false;


        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
            _phaseMachine = new StateMachine<Phase>(Phase.NOT_STARTED);

            _isGuestReady = false;
        }

        public IEnumerator CreateDecks()
        {
            _masterDM = Instantiate(MasterDMPrefab, transform.position, Quaternion.identity, this.transform);
            _guestDM = Instantiate(GuestDMPrefab, transform.position, Quaternion.identity, this.transform);

            _masterDM.InitializeCards(PlayerManager.Instance.MainPlayer);
            _guestDM.InitializeCards(PlayerManager.Instance.OtherPlayer);

            _masterDM.EnergyTracker.ConnectEvents();
            _guestDM.EnergyTracker.ConnectEvents();

            yield return new WaitForSeconds(1);
            _masterDM.DrawCardsAtTurnStart();
            _guestDM.DrawCardsAtTurnStart();
            _guestDM.FlipAllCardsToBack();

            _masterDM.CreateCardAnimationCommunicator();
        }

        public void SetUpBoard()
        {
            FindBoardView();
            SetBoardViewMode();
            BoardView.SpawnBoardSquares();
            BoardView.SpawnKings();
            BoardView.FadeGhostGrid(1);
            CreateEndGameUI();
        }

        public void SetUpTurnSystem()
        {
            _currentFactionTurn = Faction.WHITE;
            _currentTurnCount = new IntValue(1, DuelSettings.Turn);
        }


        public void EndTurn()
        {
            Debug.LogWarning("End Turn");
            if (IsMainPlayerTurn)//If the player using this client initiate the end turn function.
            {
                photonView.RPC(nameof(RPC_EndTurn), RpcTarget.All);
            }
        }

        [PunRPC]
        public void RPC_EndTurn()
        {
            //TODO this must always be black.
            if (_currentFactionTurn == Faction.BLACK)//Add turn count before changing turn faction, if the last faction to move was black.
            {
                _currentTurnCount.CurrentValue += 1;
                CheckForEndGame();
            }
            _currentFactionTurn = _currentFactionTurn == Faction.WHITE ? Faction.BLACK : Faction.WHITE;
            IncreaseTurnStartEnergy();

            if (IsMainPlayerTurn)
            {
                Debug.Log("Is MainPlayerTurn: " + IsMainPlayerTurn + "Draw 1 more card");
                MasterDM.DrawCards(1);
            }
            GameEventList.Instance.TurnChanged.RaiseEvent();
        }

        private void CheckForEndGame()
        {
            if (!ReachedFinalTurn())
            {
                Announcer.Instance.Announce($"{_currentFactionTurn.ToString()} to move.");
            }
            else
            {
                HandleEndgame();
            }
        }

        /// <summary>
        /// Called when duel ends.
        /// </summary>
        private void HandleEndgame()
        {
            BoardView.FadeGhostGrid(0);
            GameEventList.Instance.MatchEnded.RaiseEvent();

            //TODO Calulcate points.
            int whiteCount = _boardView.GetWhiteSquareCount();
            int blackCount = _boardView.GetBlackSquareCount();

            //Announce winner.
            _endGameUI.gameObject.SetActive(true);
            _endGameUI.PlayGameEndAnimation(whiteCount, blackCount);
        }


        private void IncreaseTurnStartEnergy()
        {
            if (PlayerManager.Instance.MainPlayer.Faction == _currentFactionTurn)
            {
                PlayerManager.Instance.MainPlayer.Data.Energy.CurrentValue += 3;
            }
            else
            {
                PlayerManager.Instance.OtherPlayer.Data.Energy.CurrentValue += 3;
            }
        }

        /// <summary>
        /// Ready guest player ready. Networked.
        /// </summary>
        public void GetGuestReady()
        {
            if (PhotonNetwork.IsMasterClient == false)
            {
                photonView.RPC(nameof(RPC_SetGuestReadyStatus), RpcTarget.All, true);
            }
        }

        [PunRPC]
        private void RPC_SetGuestReadyStatus(bool status)
        {
            _isGuestReady = status;
            if (_isGuestReady)
                GameEventList.Instance.GuestReadied.RaiseEvent();
            else
                GameEventList.Instance.GuestUnReadied.RaiseEvent();
        }

        /// <summary>
        /// Unready guest player. Networked.
        /// </summary>
        public void UnreadyGuest()
        {
            photonView.RPC(nameof(RPC_SetGuestReadyStatus), RpcTarget.All, false);
        }

        public void CreateBoardView()
        {
            if (_boardView == null)
            {
                BoardPosition = GameObject.FindGameObjectWithTag("BoardPosition").transform;
                _boardView = PhotonNetwork.Instantiate(BoardViewPrefabPath, BoardPosition.position, Quaternion.identity).GetComponent<BoardView>();
            }
            else
                Debug.LogWarning("A BoardView exists");
        }

        public void DestroyBoardView()
        {
            if (_boardView != null)
                Destroy(_boardView.gameObject);
        }

        public void DestroyDeckManagers()
        {
            if (_masterDM != null && _guestDM != null)
            {
                Destroy(_masterDM.gameObject);
                Destroy(_guestDM.gameObject);
            }
        }

        public void CreateEndGameUI()
        {
            if (_endGameUI == null)
                _endGameUI = Instantiate(EndGameUIPrefab, Vector3.zero, Quaternion.identity).GetComponent<EndGameUI>();
            _endGameUI.gameObject.SetActive(false);
        }


        public void FindBoardView()
        {
            _boardView = FindObjectOfType<BoardView>();
        }

        public void SetBoardViewMode()
        {
            _boardView.ViewMode = PlayerManager.Instance.MainPlayer.Faction == Faction.WHITE ? BoardViewMode.WHITE : BoardViewMode.BLACK;
        }

        private bool ReachedFinalTurn()
        {
            return _currentTurnCount.CurrentValue == DuelSettings.Turn;
        }

        public BoardView BoardView { get => _boardView; }
        public StateMachine<Phase> PhaseMachine { get => _phaseMachine; }
        public Faction CurrentFactionTurn { get => _currentFactionTurn; }

        public bool IsMainPlayerTurn => _currentFactionTurn == PlayerManager.Instance.MainPlayer.Faction;
        public bool IsOtherPlayerTurn => _currentFactionTurn == PlayerManager.Instance.OtherPlayer.Faction;

        public bool IsGuestReady { get => _isGuestReady; }
        public DeckManager MasterDM { get => _masterDM; }
        public DeckManager GuestDM { get => _guestDM; }
        public IntValue CurrentTurnCount { get => _currentTurnCount; set => _currentTurnCount = value; }
    }


    public enum Phase
    {
        NOT_STARTED,
        DRAW,
        ACTION,
        END,
        CHOOSE_PIECE,
        CHOOSE_SQUARE
    }


    public enum Faction
    {
        BLACK, WHITE, NONE
    }
}

