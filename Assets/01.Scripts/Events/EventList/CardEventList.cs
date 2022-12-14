using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    public class CardEventList
    {
        [Header("Card")]
        public CardEventChannel CardDrawn;
        public CardEventChannel CardUsed;
        public CardEventChannel CardDiscarded;

        [Header("Game Object")]
        public CardEventChannel PointerEnter;
        public CardEventChannel PointerExit;
        public CardEventChannel PointerBeginDrag;
        public CardEventChannel PointerDragging;
        public CardEventChannel PointerSelect;

        public static CardEventList Instance;

        public CardEventList()
        {
            CreateInstance();
        }

        private void CreateInstance()
        {
            CardDrawn = new CardEventChannel();
            CardUsed = new CardEventChannel();
            CardDiscarded = new CardEventChannel();

            PointerEnter = new CardEventChannel();
            PointerExit = new CardEventChannel();
            PointerBeginDrag = new CardEventChannel();
            PointerDragging = new CardEventChannel();
            PointerSelect = new CardEventChannel();
        }
    }
}
