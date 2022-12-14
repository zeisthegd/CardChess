using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Penwyn.Game
{
    public class VoidEventChannel
    {
        public event UnityAction OnEventRaised;
        public void RaiseEvent()
        {
            if (OnEventRaised != null)
                OnEventRaised?.Invoke();
        }
    }

}
