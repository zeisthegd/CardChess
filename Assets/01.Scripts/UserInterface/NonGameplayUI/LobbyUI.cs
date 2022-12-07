using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon;
using Photon.Pun;

using TMPro;
using Photon.Realtime;

using Penwyn.Game;
using Penwyn.Tools;

namespace Penwyn.UI
{
    public class LobbyUI : MonoBehaviourPunCallbacks
    {
        [SerializeField] TMP_InputField newRoomTxt;
        [SerializeField] TMP_InputField searchPasscodeTxt;

        [SerializeField] Button createRoomBtn;
        [SerializeField] Button searchPasscodeBtn;
        private void Start()
        {
            NetworkManager.Instance.JoinLobby();
            SetUpButtons();
            newRoomTxt.text = Randomizer.RandomString(5);
        }

        private void SetUpButtons()
        {
            createRoomBtn.onClick.RemoveAllListeners();
            searchPasscodeBtn.onClick.RemoveAllListeners();

            createRoomBtn.onClick.AddListener(() => { NetworkManager.Instance.CreateRoom(newRoomTxt.text); });
            searchPasscodeBtn.onClick.AddListener(() => { NetworkManager.Instance.SearchAndJoinRoom(searchPasscodeTxt.text); });
        }

    }

}