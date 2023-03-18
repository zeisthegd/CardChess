using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    [CreateAssetMenu(menuName = "Settings/Match")]
    public class MatchSettings : ScriptableObject
    {
        public int Turn = 20;
        public bool IsInfinite = false;
    }
}

