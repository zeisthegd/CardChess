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
        public TMP_Text PasscodeTxt;
        public Button OpenSettingsBtn;
        public Button StartMatchBtn;
        public Button ReadyButton;

        [Header("Score")]
        public TMP_Text HostTeamScore;
        public TMP_Text GuestTeamScore;
        [Header("Turns")]
        public TMP_Text TurnTextPrefab;
        public TMP_Text CurrentTurn;
        public Transform Container;

        void Start()
        {
            if (PhotonNetwork.InRoom)
                PasscodeTxt.text = (string)PhotonNetwork.CurrentRoom.CustomProperties["Passcode"];

            if (!PhotonNetwork.IsMasterClient)
            {
                OpenSettingsBtn.gameObject.SetActive(false);
                StartMatchBtn.gameObject.SetActive(false);
                ReadyButton.gameObject.SetActive(true);
                SetUpReadyButton();
            }
            else
            {
                ReadyButton.gameObject.SetActive(false);
            }
        }

        public void StartMatch()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameManager.Instance.StartGame();
                if (GameManager.Instance.CanStartGame)
                {
                    OpenSettingsBtn.gameObject.SetActive(false);
                    StartMatchBtn.gameObject.SetActive(false);
                }
            }
        }

        public virtual void OnGameStarted()
        {
            this.gameObject.SetActive(false);
        }

        public virtual void SetUpReadyButton()
        {
            ReadyButton.GetComponentInChildren<TMP_Text>().text = DuelManager.Instance.IsGuestReady ? "UNREADY!" : "READY!";
        }

        public virtual void ReadyButtonClicked()
        {
            if (DuelManager.Instance.IsGuestReady)
                DuelManager.Instance.UnreadyGuest();
            else
                DuelManager.Instance.GetGuestReady();
            SetUpReadyButton();
        }


        void OnEnable()
        {
            GameEventList.Instance.MatchStarted.OnEventRaised += OnGameStarted;
        }

        void OnDisable()
        {
            GameEventList.Instance.MatchStarted.OnEventRaised -= OnGameStarted;
        }
    }

}

