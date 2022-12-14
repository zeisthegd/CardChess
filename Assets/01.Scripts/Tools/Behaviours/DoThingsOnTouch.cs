using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Penwyn.Tools
{
    public class DoThingsOnTouch : MonoBehaviour
    {
        public UnityEvent ActionsOnTouch;
        public LayerMask TargetMask;
        public bool DisableAfterTouch = true;

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            if (TargetMask.Contains(col.gameObject.layer))
            {
                ActionsOnTouch?.Invoke();
                if (DisableAfterTouch)
                    gameObject.SetActive(false);
                //TODO Play sound.
            }
        }
    }
}

