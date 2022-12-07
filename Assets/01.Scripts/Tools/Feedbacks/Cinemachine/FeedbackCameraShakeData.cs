using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Tools
{
    [CreateAssetMenu(menuName = "FeedbackData/Cinemachine/Shake")]
    public class FeedbackCameraShakeData : ScriptableObject
    {
        public float Duration = 0.5F;
        public float Amplitude = 2F;
        public float Frequency = 2F;
    }
}
