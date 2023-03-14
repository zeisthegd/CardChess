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
        public Sprite WhiteHighlight;
        public Sprite WhiteBack;


        public Sprite BlackFront;
        public Sprite BlackHighlight;
        public Sprite BlackBack;

        public Sprite GetFrontSprite(Faction faction)
        {
            return faction == Faction.WHITE ? WhiteFront : BlackFront;
        }

        public Sprite GetHighlightSprite(Faction faction)
        {
            return faction == Faction.WHITE ? WhiteHighlight : BlackHighlight;
        }

        public Sprite GetBackSprite(Faction faction)
        {
            return faction == Faction.WHITE ? WhiteBack : BlackBack;
        }
    }

}
