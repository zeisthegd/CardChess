using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Penwyn.Game
{
    public class DuelistEventChannel
    {
        public event UnityAction<Duelist> OnEventRaised;
        public void RaiseEvent(Duelist card)
        {
            if (OnEventRaised != null)
                OnEventRaised?.Invoke(card);
        }
    }

}
