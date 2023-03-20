using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

using Penwyn.Game;


public class NetworkEventHandler : MonoBehaviourPunCallbacks
{
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log($"Connected: <color=green>{PhotonNetwork.NickName}</color>");
        NetworkEventList.Instance.MasterConnected.RaiseEvent();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log($"Joined Room: <color=green>{PhotonNetwork.CurrentRoom.Name}</color>");
        SceneManager.Instance.LoadRoomScene();
        NetworkEventList.Instance.RoomJoined.RaiseEvent();
    }

    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        NetworkManager.Instance.Disconnect();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.Instance.LoadLobbyScene();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        NetworkEventList.Instance.PlayerEnteredRoom.RaiseEvent(newPlayer);
        Debug.Log("Entering: " + newPlayer.NickName);

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        NetworkEventList.Instance.PlayerLeftRoom.RaiseEvent(otherPlayer);
        Debug.Log("Leaving: " + otherPlayer.NickName);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        // Destroy(GameManager.Instance.gameObject);
        //  GameManager.Instance = null;
        SceneManager.Instance.LoadTitleScene();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        NetworkManager.Instance.RoomList = roomList;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }

}
