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
        public TMP_Text HostNameTxt;
        public TMP_Text GuestNameTxt;
        public Button OpenSettingsBtn;
        public Button StartMatchBtn;
        public Button ReadyButton;

        void Start()
        {
            if (PhotonNetwork.InRoom)
            {
                PasscodeTxt.text = "CODE: " + (string)PhotonNetwork.CurrentRoom.CustomProperties["Passcode"];
                DisplayEnteredPlayerName(PhotonNetwork.MasterClient);
            }

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

        private void DisplayEnteredPlayerName(Photon.Realtime.Player player)
        {
            if (player.IsMasterClient)
            {
                HostNameTxt?.SetText($"Host: \n{player.NickName}");
            }
            else
            {
                GuestNameTxt?.SetText($"Guest: \n{player.NickName}");
            }
        }


        void OnEnable()
        {
            GameEventList.Instance.MatchStarted.OnEventRaised += OnGameStarted;
            NetworkEventList.Instance.PlayerEnteredRoom.OnEventRaised += DisplayEnteredPlayerName;
        }

        void OnDisable()
        {
            GameEventList.Instance.MatchStarted.OnEventRaised -= OnGameStarted;
            NetworkEventList.Instance.PlayerEnteredRoom.OnEventRaised += DisplayEnteredPlayerName;

        }
    }

}

