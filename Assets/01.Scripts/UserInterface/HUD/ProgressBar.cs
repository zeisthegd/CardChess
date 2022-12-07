using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using DG.Tweening;
using NaughtyAttributes;

public class ProgressBar : MonoBehaviour
{
    [Header("Sliders")]
    public Slider ActualValue;
    public Slider LostValue;

    [Header("Times")]
    public float SetDuration = 0.5F;
    public float LostDuration = 0.5F;
    public float DelayBeforeDeplete = 0;

    [Header("Value's Text")]
    public ValueTextType TextType = ValueTextType.CurrentValueOnly;
    public TMP_Text ValueText;

    protected virtual void Update()
    {
        UpdateText();
    }

    public virtual void SetValue(float newValue)
    {
        if (newValue < LostValue.value)
            StartCoroutine(SetLostValue(newValue));
        else if (LostValue != null)
            LostValue.value = newValue;

        if (ActualValue != null)
            SetActualValue(newValue);
    }

    /// <summary>
    /// Slowly show the lost progress.
    /// </summary>
    protected virtual IEnumerator SetLostValue(float newValue)
    {
        LostValue?.DOKill();
        if (DelayBeforeDeplete > 0)
            yield return new WaitForSeconds(DelayBeforeDeplete);
        LostValue?.DOValue(newValue, LostDuration);
    }


    /// <summary>
    /// Slowly show the lost progress.
    /// </summary>
    protected virtual void SetActualValue(float newValue)
    {
        ActualValue?.DOKill();
        ActualValue?.DOValue(newValue, SetDuration);
    }

    /// <summary>
    /// Set the max values of 2 sliders.
    /// </summary>
    public virtual void SetMaxValue(float newMaxValue)
    {
        if (ActualValue != null)
            ActualValue.maxValue = newMaxValue;
        if (LostValue != null)
            LostValue.maxValue = newMaxValue;
    }

    public virtual void SetWidth(float newWidth)
    {
        Vector2 newSize = new Vector2(newWidth, ActualValue.GetComponent<RectTransform>().sizeDelta.y);
        if (ActualValue != null)
            ActualValue.GetComponent<RectTransform>().sizeDelta = newSize;
        if (LostValue != null)
            LostValue.GetComponent<RectTransform>().sizeDelta = newSize;
    }

    public virtual void UpdateText()
    {
        if (ValueText != null)
        {
            ValueText.text = GetText();
        }
    }

    public virtual string GetText()
    {
        if (TextType == ValueTextType.CurrentValueOnly)
            return $"{ActualValue.value.ToString("0.0")}";
        if (TextType == ValueTextType.BothCurrentAndMax)
            return $"{ActualValue.value.ToString("0.0")}/{ActualValue.maxValue.ToString("0.0")}";
        return "";
    }

    public enum ValueTextType
    {
        CurrentValueOnly,
        BothCurrentAndMax
    }
}
