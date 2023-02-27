using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Game;

namespace Penwyn.Tools
{
    [RequireComponent(typeof(ObjectPooler))]
    public class SpriteGhostTrail : MonoBehaviour
    {
        public GameObject GhostPrefab;
        public int GhostCount = 5;
        public float Delay = 0.01F;
        public SortingLayer Layer;
        public Color Color;
        public Material Material;

        protected float _delta;
        protected SpriteRenderer _targetRenderer;
        protected ObjectPooler _ghostPooler;

        protected virtual void Awake()
        {
            _targetRenderer = GetComponent<SpriteRenderer>();
            if (GhostPrefab != null)
            {
                _ghostPooler = gameObject.AddComponent<ObjectPooler>();
                _ghostPooler.InitAtStart = false;
                _ghostPooler.NestPoolBelowThis = false;
                _ghostPooler.UseSharedInstance = true;
                _ghostPooler.NestObjectsInPool = true;
                _ghostPooler.Size = GhostCount;
                _ghostPooler.ObjectToPool = GhostPrefab;
                _ghostPooler.Init();
            }
        }

        protected virtual void Update()
        {
            if (_delta > 0)
            {
                _delta -= Time.deltaTime;
            }
            else
            {
                _delta = Delay;
                CreateGhost();
            }
        }

        public virtual void CreateGhost()
        {
            GameObject ghostObject = _ghostPooler.PullOneObject().gameObject;
            ghostObject.gameObject.SetActive(true);
            ghostObject.transform.localScale = _targetRenderer.gameObject.transform.localScale;
            ghostObject.transform.position = _targetRenderer.gameObject.transform.position;
            ghostObject.transform.rotation = _targetRenderer.gameObject.transform.rotation;

            SpriteRenderer ghostObjectRenderer = ghostObject.GetComponent<SpriteRenderer>();
            ghostObjectRenderer.sprite = _targetRenderer.sprite;
            ghostObjectRenderer.sortingLayerID = _targetRenderer.sortingLayerID;
            ghostObjectRenderer.sortingOrder = -1;
            ghostObjectRenderer.color = Color;
            if (Material)
                ghostObjectRenderer.material = Material;
        }

    }
}


