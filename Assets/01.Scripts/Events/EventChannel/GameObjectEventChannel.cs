using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Penwyn.Game
{

    public class GameObjectEventChannel
    {
        public event UnityAction<GameObject> OnEventRaised;
        public void RaiseEvent(GameObject gameObject)
        {
            if (OnEventRaised != null)
                OnEventRaised?.Invoke(gameObject);
        }
    }

}