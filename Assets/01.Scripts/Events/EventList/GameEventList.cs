using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    public class GameEventList
    {
        public VoidEventChannel CombatStart;
        public VoidEventChannel ProtagonistWon;
        public VoidEventChannel ProtagonistLost;
        public VoidEventChannel NodeEntered;
        public VoidEventChannel NodeCompleted;

        public static GameEventList Instance;

        public GameEventList()
        {
            CombatStart = new VoidEventChannel();
            ProtagonistWon = new VoidEventChannel();
            ProtagonistLost = new VoidEventChannel();
            NodeEntered = new VoidEventChannel();
            NodeCompleted = new VoidEventChannel();
        }
    }

}
