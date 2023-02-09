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

namespace Penwyn.Game
{

    public class GameManager : MonoBehaviourPun
    {
        [Header("Managers")]
        [Expandable] public NetworkManager NetworkManager;
        [Expandable] public CameraManager CameraManager;
        [Expandable] public SceneManager SceneManager;
        [Expandable] public PlayerManager PlayerManager;

        [Expandable] public InputManager InputManager;

        public AudioPlayer AudioPlayer;
        public DuelManager DuelManager;
        public LevelManager LevelManager;
        public DeckManager DeckManager;

        [Expandable] public MatchSettings MatchSettings;

        public static GameManager Instance;
        public event UnityAction GameStarted;
        protected GameState _gameState;
        public GameMode _mode = GameMode.PVP;


        void Awake()
        {
            CheckSingleton();
        }

        void Start()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoad;
        }

        public virtual void RPC_StartGame()
        {
            photonView.RPC(nameof(StartGame), RpcTarget.All);
        }


        [PunRPC]
        public virtual void StartGame()
        {
            StartCoroutine(StartGameCoroutine());
            GameStarted?.Invoke();
        }


        /// <summary>
        /// Disable input and load the first level.
        /// Create turns queue.
        /// Load the first turn;
        /// </summary>
        public IEnumerator StartGameCoroutine()
        {
            _gameState = GameState.BoardLoading;
            PlayerManager.CreatePlayers(_mode);

            DuelManager.FindBoardView();
            DuelManager.SetBoardViewMode();
            DuelManager.BoardView.SpawnBoardSquares();
            DuelManager.BoardView.SpawnKings();

            yield return new WaitForSeconds(1);

            DeckManager.InitializeCards();

            yield return new WaitForSeconds(1);
            DeckManager.DrawCardsAtTurnStart();
            _gameState = GameState.Started;
            Announcer.Instance.Announce($"Hey WHITE, you move.");
            GameEventList.Instance.CombatStart.RaiseEvent();
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
            DeckManager = FindObjectOfType<DeckManager>();
            if (PhotonNetwork.IsMasterClient)
                DuelManager.CreateBoardView();
        }

        void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == SceneManager.RoomSceenName)
            {
                OnRoomSceneLoaded();
            }
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
