using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;
namespace Penwyn.Game
{
    public class ObjectPool : MonoBehaviour
    {
        [ReadOnly] public List<GameObject> PooledObjects;

        [Button("Enable All Objects", EButtonEnableMode.Playmode)]
        public virtual void EnableAllObjects()
        {
            for (int i = 0; i < PooledObjects.Count; i++)
            {
                PooledObjects[i].SetActive(true);
            }
        }

        [Button("Disable All Objects", EButtonEnableMode.Playmode)]
        public virtual void DisableAllObjects()
        {
            for (int i = 0; i < PooledObjects.Count; i++)
            {
                PooledObjects[i].SetActive(true);
            }
        }

        [Button("Clear Pool", EButtonEnableMode.Playmode)]
        public virtual void Clear()
        {
            for (int i = 0; PooledObjects.Count > 0;)
            {
                GameObject pooledObject = PooledObjects[i];
                PooledObjects.Remove(pooledObject);
                Destroy(pooledObject);
            }
        }
    }
}

