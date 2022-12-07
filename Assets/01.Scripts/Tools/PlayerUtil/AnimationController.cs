using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Penwyn.Tools
{
    public class AnimationController : MonoBehaviour
    {
        private Animator animator;
        void Start()
        {
            animator = GetComponent<Animator>();
        }
        void Update()
        {
            if (animator == null)
                animator = GetComponentInChildren<Animator>();
        }

        public void SetRunning(bool value)
        {
            Animator.SetBool("isRunning", value);
        }

        public Animator Animator
        {
            get
            {
                if (animator == null)
                {
                    animator = GetComponentInChildren<Animator>();
                }
                return animator;
            }
        }
    }
}
