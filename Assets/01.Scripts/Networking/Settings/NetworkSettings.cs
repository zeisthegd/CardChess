using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{

    [CreateAssetMenu(menuName = "Settings/Network")]
    public class NetworkSettings : ScriptableObject
    {
        public string GameVersion;
        public string NickName;
        public bool AutomaticallySyncScene;
        public bool OfflineMode;
    }
}
