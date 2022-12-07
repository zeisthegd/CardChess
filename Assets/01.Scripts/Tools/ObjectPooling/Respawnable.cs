using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using NaughtyAttributes;
using Penwyn.Tools;

namespace Penwyn.Game
{
    public class Respawnable : MonoBehaviourPun
    {
        public float DeathTime = 10;

        protected float _deathTime;

        public virtual void Respawn()
        {
            photonView.RPC(nameof(RPC_Respawn), RpcTarget.All);
        }

        [PunRPC]
        public virtual void RPC_Respawn()
        {
            this.gameObject.SetActive(true);
        }

        protected virtual void OnDisable()
        {
            RespawnManager.Instance?.Respawn(this);
        }
    }
}
