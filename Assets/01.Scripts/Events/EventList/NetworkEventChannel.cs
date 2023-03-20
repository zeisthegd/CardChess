using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    public class NetworkEventList
    {
        public VoidEventChannel MasterConnected;
        public VoidEventChannel RoomJoined;
        public PhotonPlayerEventChannel PlayerEnteredRoom;
        public PhotonPlayerEventChannel PlayerLeftRoom;
        public static NetworkEventList Instance;

        public NetworkEventList()
        {
            MasterConnected = new VoidEventChannel();
            RoomJoined = new VoidEventChannel();
            PlayerEnteredRoom = new PhotonPlayerEventChannel();
            PlayerLeftRoom = new PhotonPlayerEventChannel();
        }
    }
}
