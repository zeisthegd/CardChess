using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Penwyn.Tools;
using DG.Tweening;
namespace Penwyn.UI
{
    public class ValueBar : MonoBehaviour
    {
        [SerializeField] Color lowColor;
        [SerializeField] Color highColor;
        [SerializeField] Vector3 offset;
        [SerializeField] Slider slider;


        void Update()
        {
            slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
        }

        public void SetUp(FloatValue value)
        {
            slider.DOComplete();
            slider.gameObject.SetActive(false);
        }

        public void SetValue(FloatValue value)
        {
            slider.DOKill();
            slider.gameObject.SetActive(true);
            slider.maxValue = value.BaseValue;
            slider.DOValue(value.CurrentValue, 0.25F);
            slider.transform.DOScale(1, 3).onComplete += () =>
            {
                slider.gameObject.SetActive(false);
            };

            slider.fillRect.GetComponent<Image>().color = Color.Lerp(lowColor, highColor, value.NormalizedValue);

        }

        public Slider Slider { get => slider; set => slider = value; }
    }
}