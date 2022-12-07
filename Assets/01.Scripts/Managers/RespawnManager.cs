using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Tools;
namespace Penwyn.Game
{
    public class RespawnManager : SingletonMonoBehaviour<RespawnManager>
    {
        public virtual void Respawn(Respawnable objToRespawn)
        {
            objToRespawn.Invoke(nameof(Respawn), objToRespawn.DeathTime);
        }
    }
}

