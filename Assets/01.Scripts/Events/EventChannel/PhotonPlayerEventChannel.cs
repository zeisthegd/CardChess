using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Photon;
namespace Penwyn.Game
{

    public class PhotonPlayerEventChannel
    {
        public event UnityAction<Photon.Realtime.Player> OnEventRaised;
        public void RaiseEvent(Photon.Realtime.Player player)
        {
            if (OnEventRaised != null)
                OnEventRaised?.Invoke(player);
        }
    }

}