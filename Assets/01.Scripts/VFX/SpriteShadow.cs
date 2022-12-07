using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Tools
{

    public class SpriteShadow : MonoBehaviour
    {
        public Vector3 LocalOffset = new Vector3(0, -0.5F, 0);
        protected SpriteRenderer _owner;
        protected SpriteRenderer _shadow;

        void Awake()
        {
            GameObject shadow = new GameObject("SpriteShadow");
            _owner = GetComponent<SpriteRenderer>();
            _shadow = shadow.AddComponent<SpriteRenderer>();

            _shadow.sortingLayerID = _owner.sortingLayerID;
            _shadow.sortingOrder = _owner.sortingOrder - 1;
            _shadow.materials = _owner.materials;
            _shadow.color = Color.black;

            _shadow.transform.SetParent(this.transform);
            _shadow.transform.localPosition += LocalOffset;

        }

        void Update()
        {
            _shadow.sprite = _owner.sprite;
            _shadow.transform.position = _owner.transform.position + LocalOffset;
        }
    }

}
