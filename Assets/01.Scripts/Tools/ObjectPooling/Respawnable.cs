using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;
using Penwyn.Tools;

namespace Penwyn.Game
{
    public class Respawnable : MonoBehaviour
    {
        public float DeathTime = 10;

        protected float _deathTime;

        public virtual void Respawn()
        {
            this.gameObject.SetActive(true);

        }
    }
}
