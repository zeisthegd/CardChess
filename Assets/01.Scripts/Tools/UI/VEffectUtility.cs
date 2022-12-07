using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Tools
{
    public static class VEffectUtility
    {
        public static void StopAndClear(this ParticleSystem effect)
        {
            effect.Stop();
            effect.Clear();
        }

        public static void PlayAt(this ParticleSystem effect, Vector3 position)
        {
            effect.transform.position = position;
            effect.Play();
        }
    }
}
