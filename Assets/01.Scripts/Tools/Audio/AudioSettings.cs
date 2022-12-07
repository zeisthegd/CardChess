using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Tools;

namespace Penwyn.Game
{
    [CreateAssetMenu(menuName = "Settings/Audio")]
    public class AudioSettings : SingletonScriptableObject<AudioSettings>
    {
        [Range(0, 1)] public float BGMVolume;
        [Range(0, 1)] public float SFXVolume;

        public void ChangeBGMVolume(float value)
        {
            BGMVolume = value;
            GameManager.Instance.AudioPlayer.BGMSource.volume = value;
        }

        public void ChangeSFXVolume(float value)
        {
            SFXVolume = value;
            GameManager.Instance.AudioPlayer.AdjustAllSFXVolume(value);
        }

        public float BgmVolume { get => BGMVolume; }
        public float SfxVolume { get => SFXVolume; }
    }
}
