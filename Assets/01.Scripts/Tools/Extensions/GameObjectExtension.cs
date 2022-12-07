using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Tools
{
    public static class GameObjectExtension
    {
        public static T FindComponent<T>(this GameObject from, bool shouldDebug = false)
        {
            T componentFound = from.GetComponent<T>();
            if (componentFound == null)
                componentFound = from.GetComponentInChildren<T>();
            if (componentFound == null)
                componentFound = from.GetComponentInParent<T>();
            if (componentFound == null && shouldDebug)
                Debug.Log($"Component not found: {from.gameObject.name} finding {typeof(T)}");
            return componentFound;
        }

        public static void RotateZ(this GameObject gameObject, float angle)
        {
            gameObject.transform.Rotate(new Vector3(0, 0, 1), angle);
        }
    }
}
