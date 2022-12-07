using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Penwyn.Game;

namespace Penwyn.UI
{
    public class AudioSettingsUI : MonoBehaviour
    {
        [SerializeField] Slider bgmVolumeSld;
        [SerializeField] Slider sfxVolumeSld;

        void OnEnable()
        {
      //      bgmVolumeSld.value = AudioSettings.Instance.BgmVolume;
         //   sfxVolumeSld.value = AudioSettings.Instance.SfxVolume;
        }
    }

}
