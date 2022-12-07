using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Tools
{
    public class BillboardUI : MonoBehaviour
    {
        private Transform cameraTr;
        void Start()
        {
            cameraTr = Camera.main.transform;
        }

        void Update()
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * -Vector3.back,
            Camera.main.transform.rotation * -Vector3.down);
        }
    }
}
