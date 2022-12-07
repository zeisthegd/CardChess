using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using Penwyn.Tools;

namespace Penwyn.Game
{
    [CreateAssetMenu(menuName = "Managers/Network Manager")]
    public class NetworkManager : SingletonScriptableObject<NetworkManager>
    {
        public NetworkSettings NetworkSettings;
        public List<RoomInfo> RoomList = new List<RoomInfo>();

        #region Server Connections

        public void Connect()
        {
            AssignSettings();
            if (PhotonNetwork.OfflineMode == false)
                PhotonNetwork.ConnectUsingSettings();
        }

        public void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }

        public void JoinLobby()
        {
            if (PhotonNetwork.IsConnectedAndReady && !PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }
        }

        public void LeaveLobby()
        {
            if (PhotonNetwork.IsConnectedAndReady)
                PhotonNetwork.LeaveLobby();
        }

        public void LeaveRoom()
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                Debug.Log("Leaving: " + PhotonNetwork.CurrentRoom.Name);
                PhotonNetwork.LeaveRoom();
            }
        }

        public void CloseRoom()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
            }
        }

        private void AssignSettings()
        {
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.NickName = NetworkSettings.NickName;
                PhotonNetwork.GameVersion = NetworkSettings.GameVersion;
                PhotonNetwork.AutomaticallySyncScene = NetworkSettings.AutomaticallySyncScene;
                PhotonNetwork.OfflineMode = NetworkSettings.OfflineMode;
            }
        }

        #endregion

        #region Room Connections
        public void JoinRoom(string roomName)
        {
            if (PhotonNetwork.IsConnectedAndReady)
                PhotonNetwork.JoinRoom(roomName);
        }

        public void SearchAndJoinRoom(string passCode)
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                for (int i = 0; i < RoomList.Count; i++)
                {
                    if ((string)RoomList[i].CustomProperties["Passcode"] == passCode)
                    {
                        PhotonNetwork.JoinRoom(RoomList[i].Name);
                        return;
                    }
                }
                Announcer.Instance.Announce("There's no existing room with this passcode!");
            }
        }

        public void CreateRoom(string roomName)
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                RoomOptions createOpts = GetRoomOptions(roomName);
                PhotonNetwork.JoinOrCreateRoom(roomName, createOpts, TypedLobby.Default);
            }
        }

        /// <summary>
        /// Create a room option for a new room.
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        private RoomOptions GetRoomOptions(string roomName)
        {
            RoomOptions options = new RoomOptions();
            ExitGames.Client.Photon.Hashtable customProps = new ExitGames.Client.Photon.Hashtable();
            customProps["Host"] = PhotonNetwork.NickName;
            customProps["Passcode"] = GetRandomPasscode();

            options.MaxPlayers = 2;
            options.CustomRoomProperties = customProps;
            options.CleanupCacheOnLeave = false;
            options.CustomRoomPropertiesForLobby = new string[] { "Host", "Passcode" };
            return options;
        }

        /// <summary>
        /// Get a random available passcode for the newly created room.
        /// </summary>
        /// <returns></returns>
        private string GetRandomPasscode()
        {
            string passcode = null;
            while (passcode == null)
            {
                passcode = Randomizer.RandomString(6);
                if (RoomList.Find(x => (string)x.CustomProperties["Passcode"] == passcode) != null)
                {
                    passcode = null;
                }
            }
            return passcode;
        }

        #endregion
    }
}

