using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

using Photon;
using Photon.Pun;

using Penwyn.Tools;

namespace Penwyn.Game
{
    [CreateAssetMenu(menuName = "Managers/Camera Manager")]
    public class CameraManager : SingletonScriptableObject<CameraManager>
    {
        public Camera MinimapCameraPrefab;
        public CameraController PlayerCameraPrefab;
        public CameraController CurrenPlayerCam;
        public Camera CurrentUICam;


        public void SetUpCameras()
        {
            CurrentUICam = Instantiate(MinimapCameraPrefab, Camera.main.transform.position, Quaternion.identity, Camera.main.transform);
        }

        /// <summary>
        /// Create the follow camera for the local player.
        /// </summary>
        public void CreatePlayerCamera()
        {
            if (CurrenPlayerCam == null)
            {
                CurrenPlayerCam = Instantiate(PlayerCameraPrefab);
            }
        }

        /// <summary>
        /// Set up minimap for client.
        /// </summary>
        void SetUpMinimapCamera()
        {

        }
    }
}

