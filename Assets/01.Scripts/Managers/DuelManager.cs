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

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
            _phaseMachine = new StateMachine<Phase>(Phase.NOT_STARTED);
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

        public BoardView BoardView { get => _boardView; }
        public StateMachine<Phase> PhaseMachine { get => _phaseMachine; }
    }

    public enum Turn
    {
        BLACK, WHITE
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
}

