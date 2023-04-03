using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

using Photon;
using Photon.Pun;
using Photon.Realtime;

using NaughtyAttributes;

using Penwyn.Tools;
using Penwyn.UI;
using System;

namespace Penwyn.Game
{

    public class GameManager : MonoBehaviourPun
    {
        [Header("Managers")]
        [Expandable] public NetworkManager NetworkManager;
        [Expandable] public CameraManager CameraManager;
        [Expandable] public SceneManager SceneManager;
        public PlayerManager PlayerManager;

        [Expandable] public InputManager InputManager;

        public AudioPlayer AudioPlayer;
        public DuelManager DuelManager;

        [Expandable] public MatchSettings MatchSettings;

        public static GameManager Instance;
        protected GameState _gameState;
        public GameMode Mode = GameMode.PVP;


        void Awake()
        {
            CheckSingleton();
        }

        void Start()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoad;
        }

        public virtual void StartGame()
        {
            if (CheckStartDuelConditions())
            {
                photonView.RPC(nameof(RPC_StartGame), RpcTarget.All);
            }
        }


        [PunRPC]
        public virtual void RPC_StartGame()
        {
            StartCoroutine(StartGameCoroutine());
        }

        private bool CheckStartDuelConditions()
        {
            if ((PhotonNetwork.CurrentRoom.PlayerCount < 2))
            {
                Announcer.Instance.Announce("Need One More Player To Start Playing.");
                return false;
            }
            if (!DuelManager.IsGuestReady)
            {
                Announcer.Instance.Announce("Guest Is Not Ready.");
                return false;
            }
            return true;
        }


        /// <summary>
        /// Disable input and load the first level.
        /// Create turns queue.
        /// Load the first turn;
        /// </summary>
        public IEnumerator StartGameCoroutine()
        {
            _gameState = GameState.BoardLoading;

            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            PlayerManager.CreatePlayers(Mode);

            DuelManager.Instance.SetUpBoard();
            DuelManager.Instance.SetUpTurnSystem();

            yield return new WaitForSeconds(0.25F);

            StartCoroutine(DuelManager.CreateDecks());

            _gameState = GameState.Started;
            Announcer.Instance.Announce($"GAME START!.");
            GameEventList.Instance.MatchStarted.RaiseEvent();
        }

        void CheckSingleton()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        public virtual void OnRoomSceneLoaded()
        {
            _gameState = GameState.GettingReady;
            if (PhotonNetwork.IsMasterClient)
                DuelManager.CreateBoardView();
            PlayerManager.HostFaction = Faction.WHITE;
        }

        public virtual void OnPhotonRoomExit()
        {
            DuelManager.DestroyBoardView();
            DuelManager.DestroyDeckManagers();
        }

        void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == SceneManager.RoomSceenName)
            {
                OnRoomSceneLoaded();
            }
            else
            {
                OnPhotonRoomExit();
            }
        }

        public void ExitApplication()
        {
            Application.Quit();
        }

        public GameState State => _gameState;
    }

    public enum GameState
    {
        NotInRoom,
        GettingReady,
        BoardLoading,
        Started
    }

    public enum GameMode
    {
        PVP,
        PVE,
        AIvAI
    }
}
