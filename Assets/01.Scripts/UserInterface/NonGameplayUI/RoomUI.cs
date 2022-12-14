using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Photon;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

using Penwyn.Game;

namespace Penwyn.UI
{
    public class RoomUI : MonoBehaviour
    {
        [SerializeField] TMP_Text passcodeTxt;
        [SerializeField] Button openSettingsBtn;
        [SerializeField] Button startMatchBtn;

        [Header("Teams")]
        public List<TMP_Text> FirstTeamList;
        public List<TMP_Text> SecondTeamList;
        [Header("Score")]
        public TMP_Text FirstTeamScore;
        public TMP_Text SecondTeamScore;
        [Header("Turns")]
        public TMP_Text TurnTextPrefab;
        public TMP_Text CurrentTurn;
        public Transform Container;

        void Awake()
        {
            PhotonTeamsManager.PlayerJoinedTeam += ShowTeams;
        }

        void Start()
        {
            if (PhotonNetwork.InRoom)
                passcodeTxt.text = (string)PhotonNetwork.CurrentRoom.CustomProperties["Passcode"];

            if (!PhotonNetwork.IsMasterClient)
            {
                openSettingsBtn.gameObject.SetActive(false);
                startMatchBtn.gameObject.SetActive(false);
            }
        }

        public void StartMatch()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                GameManager.Instance.RPC_StartGame();
                openSettingsBtn.gameObject.SetActive(false);
                startMatchBtn.gameObject.SetActive(false);
            }
        }

        public virtual void OnGameStarted()
        {
            this.gameObject.SetActive(false);
        }

        public virtual void ShowTeams(Photon.Realtime.Player player, PhotonTeam teamJoined)
        {
            Debug.Log("Show teams!");
            Photon.Realtime.Player[] team;
            PhotonTeamsManager.Instance.TryGetTeamMembers(1, out team);
            for (int i = 0; i < team.Length; i++)
            {
                FirstTeamList[i].SetText(team[i].NickName + "");
            }
            PhotonTeamsManager.Instance.TryGetTeamMembers(2, out team);
            for (int i = 0; i < team.Length; i++)
            {
                SecondTeamList[i].SetText(team[i].NickName + "");
            }
        }

        public virtual void ShowScore()
        {
        }

        public virtual void ShowTurns()
        {
        }

        void OnEnable()
        {
            GameEventList.Instance.CombatStart.OnEventRaised += OnGameStarted;
        }

        void OnDisable()
        {
            GameEventList.Instance.CombatStart.OnEventRaised -= OnGameStarted;
        }
    }

}

