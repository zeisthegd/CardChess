using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using NaughtyAttributes;

namespace Penwyn.Game
{
    public class ObjectPooler : MonoBehaviour
    {
        [Header("Pool Object")]
        public GameObject ObjectToPool;
        public int Size = 1;

        [HorizontalLine]
        [Tooltip("Find an existing pool of the same objects and use it instead of creating a new one.")]
        public bool UseSharedInstance = false;
        public bool InitAtStart = false;
        public bool EnableObjectsAtStart = false;
        public bool NestPoolBelowThis = true;
        public bool NestObjectsInPool = true;

        protected GameObject _waitingPool;
        protected ObjectPool _objectPool;

        protected virtual void Start()
        {
            if (InitAtStart)
            {
                Init();
                if (EnableObjectsAtStart)
                    _objectPool.EnableAllObjects();
            }
        }

        [Button("Init Pool", EButtonEnableMode.Always)]
        public virtual void Init()
        {
            FindSharedPool();
            CreatePool();
            FillPool();
        }

        /// <summary>
        /// Instantiate a new pool if there's none created yet.
        /// </summary>
        public virtual void CreatePool()
        {
            if (_waitingPool == null)
            {
                _waitingPool = new GameObject(DefinePoolName());
                UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(_waitingPool, UnityEngine.SceneManagement.SceneManager.GetActiveScene());
                _objectPool = _waitingPool.AddComponent<ObjectPool>();
                _objectPool.PooledObjects = new List<GameObject>();
                ApplyNesting();
            }
        }

        public virtual void FindSharedPool()
        {
            if (UseSharedInstance)
            {
                _waitingPool = GameObject.Find(DefinePoolName());
                if (_waitingPool != null)
                {
                    _objectPool = _waitingPool.GetComponent<ObjectPool>();
                }
            }
        }

        /// <summary>
        /// Fill pool till its full.
        /// </summary>
        public virtual void FillPool()
        {
            for (int i = 0; i < Size; i++)
            {
                AddOnePoolObject();
            }
        }

        public virtual void ClearPool()
        {
            if (_objectPool != null)
                _objectPool.Clear();
        }

        public virtual GameObject PullOneObject()
        {
            foreach (GameObject item in _objectPool.PooledObjects)
            {
                if (!item.gameObject.activeInHierarchy)
                    return item;
            }
            return AddOnePoolObject();
        }

        /// <summary>
        /// Instantiate the object to pool. Then add it to the list.
        /// </summary>
        public virtual GameObject AddOnePoolObject()
        {
            if (ObjectToPool != null)
            {
                GameObject poolObject = Instantiate(ObjectToPool);
                poolObject.gameObject.SetActive(false);
                poolObject.transform.SetParent(NestObjectsInPool ? _objectPool.transform : null);
                _objectPool.PooledObjects.Add(poolObject);
                return poolObject;
            }
            Debug.LogWarning("No Object to pool. Please insert");
            return null;
        }

        public virtual bool NoPoolFound()
        {
            if (ObjectPool != null)
                FindSharedPool();
            return _objectPool == null;
        }



        /// <summary>
        /// Determine the parent of this pool.
        /// </summary>
        public virtual void ApplyNesting()
        {
            if (_waitingPool)
            {
                _waitingPool.transform.SetParent(NestPoolBelowThis ? this.transform : null);
            }
        }

        /// <summary>
        /// Type of pool and name of ObjectToPool.
        /// </summary>
        public virtual string DefinePoolName()
        {
            return $"[{this.GetType().ToString()}] +[{ObjectToPool.name}]";
        }

        public virtual void DisableAllPooledObjects()
        {
            foreach (GameObject obj in _objectPool.PooledObjects)
            {
                obj.gameObject.SetActive(false);
            }
        }

        public ObjectPool ObjectPool { get => _objectPool; }
    }
}

