using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Tools
{
    public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        static T instance = null;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    T[] objects = Resources.FindObjectsOfTypeAll<T>();
                    if (objects.Length == 0)
                    {
                        Debug.Log("Singleton Scriptable Object: there's no object of " + typeof(T).ToString());
                        return null;
                    }
                    if (objects.Length > 1)
                    {
                        Debug.Log("Singleton Scriptable Object: there're more than 1 objects of " + typeof(T).ToString());
                        return null;
                    }
                    instance = objects[0];
                }
                return instance;
            }
        }

        void OnDisable()
        {
            instance = null;
        }
    }
}
