using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Tools;
using Photon.Pun;

namespace Penwyn.Game
{
    public class TimeManager : SingletonMonoBehaviour<TimeManager>
    {
        protected PhotonView _photonView;
        protected virtual void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        void Update()
        {
            
        }

        [PunRPC]
        public virtual void ResetTime()
        {
            SetTimeScale(1);
        }

        [PunRPC]
        public virtual void StopTime()
        {
            SetTimeScale(0);
        }

        public virtual void SetTimeScale(float newTimeScale)
        {
            Time.timeScale = newTimeScale;
            PhotonNetwork.MinimalTimeScaleToDispatchInFixedUpdate = newTimeScale == 0 ? 0 : -1f;
        }

        public virtual void LocalPlayerIsActing()
        {

        }

        public virtual void OnPlayerSpawned()
        {
            ConnectEvents();
        }

        public virtual void ConnectEvents()
        {

        }

        public virtual void DisconnectEvents()
        {
            
        }
    }
}
