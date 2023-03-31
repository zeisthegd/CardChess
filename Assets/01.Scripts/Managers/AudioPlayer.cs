using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Penwyn.Tools;

namespace Penwyn.Game
{
    public class AudioPlayer : MonoBehaviour
    {
        [Header("--- Audio Sources ---")]
        public AudioSource BGMSource;
        public AudioSettings settings;

        [Header("--- BGM Clips ---")]
        public AudioClip[] BGMClips;// Background music clips
        [Header("--- SFX Clips ---")]
        public AudioClip CardHoveredSfx;
        public AudioClip PieceDeployedSfx;
        public AudioClip SquareTakenSfx;
        public AudioClip TurnStartSfx;

        [Header("--- UI SFX Clips ---")]
        public AudioClip ConfirmSfx;
        public AudioClip CancelSfx;

        protected List<AudioSource> _sfxSourceList = new List<AudioSource>();

        event UnityAction BGMEnded;

        void Start()
        {
            BGMEnded += PlayBGM;
            BGMSource.volume = settings.BgmVolume;
        }

        public void PlayBGM()
        {
            BGMSource.PlayOneShot(BGMClips[Randomizer.RandomNumber(0, BGMClips.Length)]);
            StartCoroutine(WaitForClip(BGMSource, BGMEnded));
        }

        /// <summary>
        /// Play a clip.
        /// </summary>
        public void PlaySFX(AudioClip clip, Vector3 point, bool loop = false)
        {
            if (clip != null)
            {
                var source = PlayClipAt(clip, point, settings.SfxVolume);
                _sfxSourceList.Add(source);
            }
            else
            {
                Debug.Log("Can't find AudioClip");
            }
        }

        public void PlayCardHoveredSFX()
        {
            PlaySFX(CardHoveredSfx, Camera.main.transform.position);
        }

        public void PlayPieceDeployedSfx()
        {
            PlaySFX(PieceDeployedSfx, Camera.main.transform.position);
        }

        public void PlaySquareTakenSfx()
        {
            PlaySFX(SquareTakenSfx, Camera.main.transform.position);
        }

        public void PlayConfirmSfx()
        {
            PlaySFX(ConfirmSfx, Camera.main.transform.position);
        }

        public void PlayCancelSfx()
        {
            PlaySFX(CancelSfx, Camera.main.transform.position);
        }

        public void PlayTurnStartSfx()
        {
            PlaySFX(TurnStartSfx, Camera.main.transform.position);
        }

        public void AdjustAllSFXVolume(float value)
        {
            for (int i = 0; i < _sfxSourceList.Count; i++)
            {
                if (_sfxSourceList != null)
                {
                    _sfxSourceList[i].volume = value;
                }
            }
        }

        /// <summary>
        /// Choose a random Audio clip and play it
        /// </summary>
        public AudioSource PlayClipAt(AudioClip clip, Vector3 point, float volume)
        {
            GameObject tempGO = new GameObject(clip.name);
            tempGO.transform.position = point;

            AudioSource aSource = tempGO.AddComponent<AudioSource>();
            aSource.clip = clip;
            aSource.volume = volume;
            aSource.Play();

            Destroy(tempGO, clip.length);
            return aSource;
        }

        public IEnumerator WaitForClip(AudioSource source, UnityAction action)
        {
            if (source != null)
            {
                yield return new WaitUntil(() => source != null && source.isPlaying == false);
                action?.Invoke();
            }
        }

        void OnDisable()
        {
            BGMEnded -= PlayBGM;
        }
    }
}
