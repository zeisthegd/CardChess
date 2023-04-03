using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Photon;
using Photon.Pun;

using Penwyn.Game;

namespace Penwyn.UI
{
    public class MatchOptionsUI : MonoBehaviour
    {
        public Toggle PrivateRoomTgl;
        public TMP_Dropdown TurnCountDD;
        public TMP_Dropdown HostStartFactionDD;

        public void Awake()
        {
            PrivateRoomTgl.onValueChanged.AddListener(ChangeRoomVisibility);
            TurnCountDD.onValueChanged.AddListener(ChangeMatchTurnCount);
            HostStartFactionDD.onValueChanged.AddListener(ChangeHostStartFaction);
            this.gameObject.SetActive(false);
        }

        private void ChangeRoomVisibility(bool toggleVal)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = toggleVal;
                PhotonNetwork.CurrentRoom.IsVisible = toggleVal;
            }
        }

        private void ChangeMatchTurnCount(int option)
        {
            int turnCount = int.Parse(TurnCountDD.options[option].text);
            DuelManager.Instance.DuelSettings.Turn = turnCount;
        }

        private void ChangeHostStartFaction(int option)
        {
            string chosenOption = HostStartFactionDD.options[option].text;
            if (chosenOption == Faction.WHITE.ToString())
            {
                PlayerManager.Instance.ChangeHostStartingFaction(Faction.WHITE);
            }
            else if (chosenOption == Faction.BLACK.ToString())
            {
                PlayerManager.Instance.ChangeHostStartingFaction(Faction.BLACK);
            }
        }

        public void OnDisable()
        {
            PrivateRoomTgl.onValueChanged.RemoveAllListeners();
            TurnCountDD.onValueChanged.RemoveAllListeners();
            HostStartFactionDD.onValueChanged.RemoveAllListeners();
        }
    }

}
