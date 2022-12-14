using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Penwyn.Game
{
    public class CardEventChannel
    {
        public event UnityAction<Card> OnEventRaised;
        public void RaiseEvent(Card card)
        {
            if (OnEventRaised != null)
                OnEventRaised?.Invoke(card);
        }
    }
}

