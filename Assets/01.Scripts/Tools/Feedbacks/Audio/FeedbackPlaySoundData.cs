using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;
namespace Penwyn.Tools
{
    [CreateAssetMenu(menuName = "FeedbackData/Audio/Play Sound")]
    public class FeedbackPlaySoundData : ScriptableObject
    {
        public string Name;
        public AudioClip Sound;
    }
}
