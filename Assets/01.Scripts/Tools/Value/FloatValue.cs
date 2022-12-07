using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Tools
{
    [System.Serializable]
    public class FloatValue
    {
        [SerializeField] float baseValue = 0;
        [SerializeField] float currentValue = 0;

        public void Reset()
        {
            currentValue = baseValue;
        }

        public bool ValueUnchanged()
        {
            return currentValue == baseValue;
        }

        public bool ValueIncreased()
        {
            return currentValue > baseValue;
        }

        public bool ValueDecreased()
        {
            return currentValue < baseValue;
        }

        public string GetValueText()
        {
            if (ValueDecreased())
                return $"<color=red>{currentValue}</color>";
            if (ValueIncreased())
                return $"<color=green>{currentValue}</color>";
            return $"<color=white>{currentValue}</color>";
        }

        public float CurrentValue
        {
            get => currentValue; set
            {
                currentValue = value;
            }
        }
        public float BaseValue { get => baseValue; set => baseValue = value; }
        public float NormalizedValue { get => (float)1.0 * currentValue / baseValue; }
    }
}

