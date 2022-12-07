using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using Penwyn.Game;

namespace Penwyn.UI
{
    public class RoomListingsMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField] Transform container;
        [SerializeField] RoomListing roomListingPref;
        List<RoomListing> listings;

        void Start()
        {
            listings = new List<RoomListing>();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
            ListRooms(roomList);
        }

        void ListRooms(List<RoomInfo> roomList)
        {
            foreach (RoomInfo info in roomList)
            {
                TryRemoveUnavailableRoomListing(info);
                TryCreateNewRoomListing(info);
            }
        }

        /// <summary>
        /// Removed room listings of closed, started, destroyed room.
        /// </summary>
        void TryRemoveUnavailableRoomListing(RoomInfo info)
        {
            if (info.RemovedFromList)
            {
                int roomIndex = listings.FindIndex(lst => lst.RoomInfo.Name == info.Name);
                if (roomIndex >= 0)
                {
                    RoomListing unavailRoomLst = listings[roomIndex];
                    listings.Remove(unavailRoomLst);
                    Destroy(unavailRoomLst.gameObject);
                }
            }
        }

        /// <summary>
        /// Create new rooms listing for new rooms.
        /// </summary>
        /// <param name="info"></param>
        void TryCreateNewRoomListing(RoomInfo info)
        {
            if (!info.RemovedFromList)
            {
                int existedIndex = listings.FindIndex(lst => lst.RoomInfo.Name == info.Name);
                if (existedIndex == -1)
                    CreateRoomListing(info);
                else
                    listings[existedIndex].SetRoomInfo(info);
            }
        }

        /// <summary>
        /// Create a room listing object.
        /// </summary>
        void CreateRoomListing(RoomInfo info)
        {
            RoomListing roomLst = Instantiate(roomListingPref, container.transform.position, Quaternion.identity, container);
            roomLst.SetRoomInfo(info);
            listings.Add(roomLst);
        }
    }

}