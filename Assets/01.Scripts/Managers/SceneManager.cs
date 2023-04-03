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
        public string TitleSceneName;
        public string StartMenuName;
        public string SettingsSceneName;
        public string LobbySceneName;
        public string RoomSceneName;

        #region Load Scenes
        public void LoadTitleScene()
        {
            LoadScene(TitleSceneName);
        }

        public void LoadStartMenuScene()
        {
            LoadScene(StartMenuName);
        }

        public void LoadSettingsScene()
        {
            LoadScene(SettingsSceneName);
        }

        public void LoadLobbyScene()
        {
            LoadScene(LobbySceneName);
        }

        public void LoadRoomScene()
        {
            LoadScene(RoomSceneName);
        }

        void LoadScene(string sceneName)
        {
            PhotonNetwork.LoadLevel(sceneName);
        }
        #endregion


        public void ExitGame()
        {
            NetworkManager.Instance.Disconnect();
            Application.Quit();
        }
    }
}
