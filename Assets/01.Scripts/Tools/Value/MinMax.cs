using System;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Penwyn.Tools
{
    [System.Serializable]
    public class FloatMinMax
    {
        [SerializeField] float min;
        [SerializeField] float max;

        public float GetRandomValue()
        {
            return Random.Range(min, max);
        }

        public float Min { get => min; set => min = value; }
        public float Max { get => max; set => max = value; }
    }

    [System.Serializable]
    public class IntMinMax
    {
        [SerializeField] int min;
        [SerializeField] int max;
        public int GetRandomValue()
        {
            return Random.Range(min, max);
        }

        public int Min { get => min; set => min = value; }
        public int Max { get => max; set => max = value; }
    }
}
