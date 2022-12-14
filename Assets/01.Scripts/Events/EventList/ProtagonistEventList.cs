using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    public class ProtagonistEventList
    {
        [Header("Card Usage")]
        public CardEventChannel CardChosen;
        public VoidEventChannel PressedPlayCard;
        public VoidEventChannel ReleaseCard;
        public VoidEventChannel MouseEnteredPlayZone;
        public VoidEventChannel MouseExitedPlayZone;

        [Header("Protagonist Data")]
        public VoidEventChannel EnergyAltered;
        public VoidEventChannel ProtagonistCreated;
        public GameObjectEventChannel BoughtItem;

        [Header("Game Events")]
        public VoidEventChannel TurnStart;
        public VoidEventChannel ProtagonistFallen;
        public VoidEventChannel TurnEnd;

        public static ProtagonistEventList Instance;

        public ProtagonistEventList()
        {
            CreateInstance();
        }

        private void CreateInstance()
        {
            CardChosen = new CardEventChannel();
            PressedPlayCard = new VoidEventChannel();
            ReleaseCard = new VoidEventChannel();
            MouseEnteredPlayZone = new VoidEventChannel();
            MouseExitedPlayZone = new VoidEventChannel();

            EnergyAltered = new VoidEventChannel();
            ProtagonistCreated = new VoidEventChannel();
            BoughtItem = new GameObjectEventChannel();


            TurnStart = new VoidEventChannel();
            ProtagonistFallen = new VoidEventChannel();
            TurnEnd = new VoidEventChannel();

            Instance = this;
        }
    }
}