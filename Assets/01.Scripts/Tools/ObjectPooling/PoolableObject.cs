using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

namespace Penwyn.Game
{
    public class PoolableObject : MonoBehaviour
    {
        [HorizontalLine]
        [Header("Poolable Object")]
        public bool InfiniteLifeTime = true;
        [HideIf("InfiniteLifeTime")]
        public float LifeTime = 1;

        protected virtual void Awake()
        {
        }

        protected virtual void Update()
        {

        }

        protected virtual void FixedUpdate()
        {

        }

        protected virtual void OnEnable()
        {
            if (LifeTime > 0 && !InfiniteLifeTime)
            {
                Invoke("Destroy", LifeTime);
            }
        }

        public virtual void Destroy()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnDisable()
        {
            CancelInvoke();
        }
    }
}

