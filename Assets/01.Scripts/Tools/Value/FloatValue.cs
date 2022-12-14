using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Penwyn.Tools
{
    [System.Serializable]
    public class FloatValue
    {
        [SerializeField] protected float _baseValue = 0;
        [SerializeField] protected float _currentValue = 0;

        public bool CanBeHigherThanBase = false;
        public bool CanBeNegative = false;

        public FloatValue()
        {
            BaseValue = 0;
            CurrentValue = 0;
        }

        public FloatValue(float baseValue)
        {
            BaseValue = baseValue;
            CurrentValue = 0;
        }

        public FloatValue(float currentValue, float baseValue)
        {
            BaseValue = baseValue;
            CurrentValue = currentValue;
        }

        public event UnityAction CurrentValueChanged;
        public event UnityAction ReachedZero;
        public event UnityAction ReachedMax;

        public void Reset()
        {
            _currentValue = _baseValue;
        }

        public bool ValueUnchanged()
        {
            return _currentValue == _baseValue;
        }

        public bool ValueWasIncreased()
        {
            return _currentValue > _baseValue;
        }

        public bool ValueWasDecreased()
        {
            return _currentValue < _baseValue;
        }

        public void SetBaseValue(float newBase)
        {
            _baseValue = newBase;
        }

        public void SetCurrentValue(float newCrt)
        {
            _currentValue = newCrt;

            if (CanBeHigherThanBase == false && _currentValue > _baseValue)
                _currentValue = _baseValue;
            if (CanBeNegative == false && _currentValue < 0)
                _currentValue = 0;

            CurrentValueChanged?.Invoke();

            if (_currentValue == 0)
                ReachedZero?.Invoke();
            if (_currentValue == _baseValue)
                ReachedMax?.Invoke();
        }

        public string GetValueText()
        {
            if (ValueWasIncreased())
                return $"<color=red>{_currentValue}</color>";
            if (ValueWasDecreased())
                return $"<color=green>{_currentValue}</color>";
            return $"<color=white>{_currentValue}</color>";
        }

        public float NormalizedValue { get => (float)1.0 * _currentValue / _baseValue; }
        public float BaseValue { get => _baseValue; set => SetBaseValue(value); }
        public float CurrentValue { get => _currentValue; set => SetCurrentValue(value); }
        public override string ToString()
        {
            return $"Base: {BaseValue} | Current: {CurrentValue}";
        }
    }
}

