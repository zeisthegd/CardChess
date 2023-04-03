using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Penwyn.Game;

using AudioSettings = Penwyn.Game.AudioSettings;
namespace Penwyn.UI
{
    public class AudioSettingsUI : MonoBehaviour
    {
        public SliderTMP BGMVolumeSld;
        public SliderTMP SFXVolumeSld;

        void OnEnable()
        {
            BGMVolumeSld.value = AudioSettings.Instance.BgmVolume * 10;
            SFXVolumeSld.value = AudioSettings.Instance.SfxVolume * 10;

            BGMVolumeSld.onValueChanged.AddListener(ChangeBGMVolume);
            SFXVolumeSld.onValueChanged.AddListener(ChangeSFXVolume);
        }

        private void ChangeBGMVolume(float value)
        {
            GameManager.Instance.AudioPlayer.PlayConfirmSfx();
            AudioSettings.Instance.ChangeBGMVolume(value / 10);
        }

        private void ChangeSFXVolume(float value)
        {
            AudioSettings.Instance.ChangeSFXVolume(value / 10);
        }

        void OnDisable()
        {

        }

    }

}
