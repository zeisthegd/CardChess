using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Penwyn.Tools
{
    [System.Serializable]
    public class IntValue
    {
        [SerializeField] protected int _baseValue = 0;
        [SerializeField] protected int _currentValue = 0;

        public bool CanBeHigherThanBase = false;
        public bool CanBeNegative = false;

        public IntValue()
        {
            BaseValue = 0;
            CurrentValue = 0;
        }

        public IntValue(int baseValue)
        {
            BaseValue = baseValue;
            CurrentValue = 0;
        }

        public IntValue(int currentValue, int baseValue)
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

        public void SetBaseValue(int newBase)
        {
            _baseValue = newBase;
        }

        public void SetCurrentValue(int newCrt)
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

        public string GetCurrentValueText()
        {
            if (ValueWasIncreased())
                return $"<color=#A58C27>{_currentValue}</color>";
            if (ValueWasDecreased())
                return $"<color=#9B1A0A>{_currentValue}</color>";
            return $"<color=#EFD8A1>{_currentValue}</color>";
        }

        public float NormalizedValue { get => (float)1.0 * _currentValue / _baseValue; }
        public int BaseValue { get => _baseValue; set => SetBaseValue(value); }
        public int CurrentValue { get => _currentValue; set => SetCurrentValue(value); }
        public override string ToString()
        {
            return $"Base: {BaseValue} | Current: {CurrentValue}";
        }
    }
}

