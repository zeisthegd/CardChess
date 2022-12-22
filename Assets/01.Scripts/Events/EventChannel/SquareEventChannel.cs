using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Penwyn.Game
{
    public class SquareEventChannel
    {
        public event UnityAction<Square> OnEventRaised;
        public void RaiseEvent(Square square)
        {
            if (OnEventRaised != null)
                OnEventRaised?.Invoke(square);
        }
    }
}

