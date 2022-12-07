using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using NaughtyAttributes;

using Penwyn.Tools;
using Penwyn.UI;

namespace Penwyn.Game
{
    public class DebugManager : SingletonMonoBehaviour<DebugManager>
    {
        public Transform ContentTransform;
        public ObjectPooler TextPool;

        public TMP_Text FPSText;

        [HorizontalLine]
        public bool ShowFPS = true;
        protected int _currentLast = 0;
        public virtual void Update()
        {
            LogFPS();
        }

        public virtual void LogFPS()
        {
            if (ShowFPS)
            {
                FPSText?.SetText($"{(int)(1 / Time.deltaTime)}");
            }
        }
        public void Log(object objectToLog)
        {
            if (TextPool.ObjectPool == null)
            {
                Debug.Log(objectToLog);
                return;
            }
            TMP_Text tmpText = GetLatestText();
            Debug.Log(TextPool.ObjectPool.PooledObjects.IndexOf(tmpText.gameObject));

            tmpText.gameObject.SetActive(true);
            tmpText.transform.SetParent(ContentTransform);
            tmpText.transform.localScale = Vector3.one;
            tmpText.text = objectToLog.ToString();
            tmpText.transform.SetAsFirstSibling();
        }

        public void Clear()
        {
            TextPool.ObjectPool.DisableAllObjects();
        }

        /// <returns>Top most text mesh.</returns>
        public TMP_Text GetLatestText()
        {
            for (int i = 0; i < TextPool.ObjectPool.PooledObjects.Count; i++)
            {
                if (!TextPool.ObjectPool.PooledObjects[i].gameObject.activeInHierarchy)
                    return TextPool.ObjectPool.PooledObjects[i].GetComponent<TMP_Text>();
            }
            _currentLast++;
            if (_currentLast >= TextPool.ObjectPool.PooledObjects.Count)
                _currentLast = 0;
            return TextPool.ObjectPool.PooledObjects[_currentLast + 1].GetComponent<TMP_Text>();
        }
    }
}

