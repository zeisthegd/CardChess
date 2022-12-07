using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    [CreateAssetMenu(menuName = "Settings/Match")]
    public class MatchSettings : ScriptableObject
    {
        public float LevelLoadTime = 1;
        public float PlayerPositioningTime = 1;
    }
}

