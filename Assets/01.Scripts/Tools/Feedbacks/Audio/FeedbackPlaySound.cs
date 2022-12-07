using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Game;

namespace Penwyn.Tools
{
    public class FeedbackPlaySound : Feedback
    {
        public FeedbackPlaySoundData Data;
        protected AudioSource _audioSource;
        protected static ObjectPooler AudioSourcePooler;

        protected override void Start()
        {
            base.Start();
            InitAudioSourcePooler();
        }

        public override void PlayFeedback()
        {
            StopAllCoroutines();
            _audioSource = AudioSourcePooler.PullOneObject().GetComponent<AudioSource>();
            _audioSource.GetComponent<PoolableObject>().LifeTime = Data.Sound.length;
            _audioSource.gameObject.SetActive(true);
            _audioSource.transform.position = this.transform.position;
            _audioSource.PlayOneShot(Data.Sound);
        }

        public virtual void InitAudioSourcePooler()
        {
            if (AudioSourcePooler == null)
            {
                GameObject poolerObject = new GameObject("AudioSourcePooler");
                AudioSourcePooler = poolerObject.AddComponent<ObjectPooler>();
                AudioSourcePooler.Size = 100;
                AudioSourcePooler.UseSharedInstance = true;

                AudioSourcePooler.InitAtStart = false;
                AudioSourcePooler.EnableObjectsAtStart = false;
                AudioSourcePooler.NestPoolBelowThis = true;
                AudioSourcePooler.NestObjectsInPool = true;

                GameObject audioObject = new GameObject(GetSourceName());
                audioObject.AddComponent<PoolableObject>();
                _audioSource = audioObject.AddComponent<AudioSource>();
                _audioSource.playOnAwake = true;

                AudioSourcePooler.ObjectToPool = _audioSource.gameObject;
                AudioSourcePooler.Init();
            }
        }


        protected override void OnDisable()
        {
            base.OnDisable();
        }

        public string GetSourceName()
        {
            return "AudioSource_" + Data.Name;
        }
    }
}