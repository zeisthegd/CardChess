using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;
using DG.Tweening;
using Penwyn.Game;

namespace Penwyn.Tools
{
    public class CameraController : MonoBehaviour
    {
        public CinemachineVirtualCamera VirtualCamera;
        public CinemachineBasicMultiChannelPerlin VirtualCameraNoise;

        protected virtual void Awake()
        {
            VirtualCamera = GetComponent<CinemachineVirtualCamera>();
            VirtualCameraNoise = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }


        public virtual void Follow(Transform followTrf, Transform lookAtTrf)
        {
            VirtualCamera.Follow = followTrf;
            VirtualCamera.LookAt = lookAtTrf;
        }

        public virtual void StartShaking(float amplitude, float frequency)
        {
            if (VirtualCamera != null && VirtualCameraNoise != null)
            {
                VirtualCameraNoise.m_AmplitudeGain = amplitude;
                VirtualCameraNoise.m_FrequencyGain = frequency;
            }
        }

        public virtual void SetFOV(float newFOV)
        {
            if (VirtualCamera != null)
                VirtualCamera.m_Lens.FieldOfView = newFOV;
        }

        public virtual void ChangeBodyDistance(float newDst)
        {
            if (VirtualCamera != null)
            {
                if (VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>())
                {
                    VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = newDst;
                }
            }
        }

        public virtual void ConnectEvents()
        {
        }

        public virtual void DisconnectEvents()
        {

        }

        protected virtual void OnEnable()
        {
            ConnectEvents();
        }

        protected virtual void OnDisable()
        {
            DisconnectEvents();
        }
    }
}

