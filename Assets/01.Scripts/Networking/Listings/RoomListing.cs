using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;

using TMPro;
using Penwyn.Game;

namespace Penwyn.UI
{
    public class RoomListing : MonoBehaviour
    {
        [SerializeField] TMP_Text roomNameTxt;
        [SerializeField] TMP_Text roomLeaderTxt;
        [SerializeField] TMP_Text playerCountTxt;
        [SerializeField] Button joinBtn;

        RoomInfo roomInfo;

        void Start()
        {
            joinBtn.onClick.RemoveAllListeners();
            joinBtn.onClick.AddListener(() => { NetworkManager.Instance.JoinRoom(roomNameTxt.text); });
        }

        public void SetRoomInfo(RoomInfo roomInfo)
        {
            this.roomInfo = roomInfo;
            roomNameTxt.text = roomInfo.Name;
            roomLeaderTxt.text = (string)roomInfo.CustomProperties["Host"];
            playerCountTxt.text = $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
        }



        public TMP_Text RoomNameTxt { get => roomNameTxt; }
        public TMP_Text RoomLeaderTxt { get => roomLeaderTxt; }
        public TMP_Text PlayerCountTxt { get => playerCountTxt; }
        public Button JoinBtn { get => joinBtn; }
        public RoomInfo RoomInfo { get => roomInfo; }
    }
}

