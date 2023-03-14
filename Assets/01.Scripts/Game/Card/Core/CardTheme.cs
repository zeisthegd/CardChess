using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    [CreateAssetMenu(menuName = "Card/Data/Theme", fileName = "New Card Theme")]
    public class CardTheme : ScriptableObject
    {
        [Header("Frame")]
        public Sprite WhiteFront;
        public Sprite BlackFront;
        public Sprite WhiteHighlight;
        public Sprite BlackHighlight;
        public Sprite WhiteBack;
        public Sprite BlackBack;
    }

}
