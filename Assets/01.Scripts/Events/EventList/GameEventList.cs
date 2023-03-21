using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    public class GameEventList
    {
        public VoidEventChannel MatchStarted;
        public VoidEventChannel MatchEnded;
        public VoidEventChannel TurnChanged;
        public VoidEventChannel GuestReadied;
        public VoidEventChannel GuestUnReadied;

        public static GameEventList Instance;

        public GameEventList()
        {
            MatchStarted = new VoidEventChannel();
            MatchEnded = new VoidEventChannel();
            TurnChanged = new VoidEventChannel();
            GuestReadied = new VoidEventChannel();
            GuestUnReadied = new VoidEventChannel();
        }
    }

}
