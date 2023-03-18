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
            _currentTurnCount = new IntValue(1, DuelSettings.Turn);
        }

        public void EndTurn()
        {
            if (IsMainPlayerTurn)//If the player using this client initiate the end turn function.
            {
                photonView.RPC(nameof(RPC_EndTurn), RpcTarget.All);
            }
        }

        [PunRPC]
        public void RPC_EndTurn()
        {
            CheckForEndGame();
            if (_currentFactionTurn == Faction.BLACK)//Add turn count before changing turn faction, if the last faction to move was black.
                _currentTurnCount.CurrentValue += 1;
            _currentFactionTurn = _currentFactionTurn == Faction.WHITE ? Faction.BLACK : Faction.WHITE;
            IncreaseTurnStartEnergy();
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

        private void HandleEndgame()
        {
            GameEventList.Instance.MatchEnded.RaiseEvent();
            //TODO Calulcate points.
            int whiteCount = _boardView.GetWhiteSquareCount();
            int blackCount = _boardView.GetBlackSquareCount();
            Debug.Log("WHITE: " + whiteCount);
            Debug.Log("BLACK: " + blackCount);
            //Announce winner.
            if (whiteCount > blackCount)
                Announcer.Instance.Announce($"White Won: {whiteCount} vs. {blackCount}");
            else if (blackCount > whiteCount)
                Announcer.Instance.Announce($"Black Won: {blackCount} vs. {whiteCount}");
            else
                Announcer.Instance.Announce($"Draw: {whiteCount} vs. {blackCount}");
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
                Debug.Log(GameManager.Instance.Mode);
                photonView.RPC(nameof(RPC_GetGuestReady), RpcTarget.All);
            }
        }

        [PunRPC]
        private void RPC_GetGuestReady()
        {
            _isGuestReady = true;
        }

        /// <summary>
        /// Unready guest player. Networked.
        /// </summary>
        public void UnreadyGuest()
        {
            photonView.RPC(nameof(RPC_UnreadyGuest), RpcTarget.All);
        }

        [PunRPC]
        private void RPC_UnreadyGuest()
        {
            _isGuestReady = false;
        }


        public void CreateBoardView()
        {
            BoardPosition = GameObject.FindGameObjectWithTag("BoardPosition").transform;
            _boardView = PhotonNetwork.Instantiate(BoardViewPrefabPath, BoardPosition.position, Quaternion.identity).GetComponent<BoardView>();
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

