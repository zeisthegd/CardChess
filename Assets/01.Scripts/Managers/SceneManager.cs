using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

using Photon;
using Photon.Pun;

using Penwyn.Tools;

namespace Penwyn.Game
{
    [CreateAssetMenu(menuName = "Managers/Scene Manager")]
    public class SceneManager : SingletonScriptableObject<SceneManager>
    {
        [Header("Scene names")]
        public string TitleSceenName;
        public string LobbySceenName;
        public string RoomSceenName;
        public string MatchSceenName;

        #region Load Scenes
        public void LoadTitleScene()
        {
            LoadScene(TitleSceenName);
        }

        public void LoadLobbyScene()
        {
            LoadScene(LobbySceenName);
        }

        public void LoadRoomScene()
        {
            LoadScene(RoomSceenName);
        }

        void LoadScene(string sceneName)
        {
            PhotonNetwork.LoadLevel(sceneName);
        }
        #endregion

        #region Load Level

        public void LoadMatchScene()
        {
            PhotonNetwork.LoadLevel(MatchSceenName);
        }

        #endregion


        public void ExitGame()
        {
            NetworkManager.Instance.Disconnect();
            Application.Quit();
        }
    }
}
