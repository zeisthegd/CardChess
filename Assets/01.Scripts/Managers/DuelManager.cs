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
        private BoardView _boardView;
        private PhotonView photonView;

        private StateMachine<Phase> _phaseMachine;
        private Faction _currentFactionTurn = Faction.WHITE;

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
            _phaseMachine = new StateMachine<Phase>(Phase.NOT_STARTED);
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
            _currentFactionTurn = _currentFactionTurn == Faction.WHITE ? Faction.BLACK : Faction.WHITE;
            Announcer.Instance.Announce($"Hey {_currentFactionTurn.ToString()}, you move.");
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

        public BoardView BoardView { get => _boardView; }
        public StateMachine<Phase> PhaseMachine { get => _phaseMachine; }
        public Faction CurrentFactionTurn { get => _currentFactionTurn; }

        public bool IsMainPlayerTurn => _currentFactionTurn == PlayerManager.Instance.MainPlayer.Faction;
        public bool IsOtherPlayerTurn => _currentFactionTurn == PlayerManager.Instance.OtherPlayer.Faction;
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

