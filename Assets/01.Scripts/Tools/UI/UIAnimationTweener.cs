using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
namespace Penwyn.Tools
{
    public class UIAnimationTweener : MonoBehaviour
    {
        [SerializeField] RectTransform ui;

        void Start()
        {
            if (ui == null)
                ui = GetComponent<RectTransform>();
            //InputReader.Instance.CloseUI += Hide;
        }

        public void DynamicShowHide()
        {
            if (ui.gameObject.activeInHierarchy)
                Hide();
            else
                Show();
        }

        public void Show()
        {
            ui.DOKill();
            ui.gameObject.SetActive(true);
            Sequence showSqn = DOTween.Sequence();
            showSqn.Append(ui.DOScale(Vector3.one, 0.25F));

        }

        public void Hide()
        {
            ui.DOKill();
            Sequence showSqn = DOTween.Sequence();
            showSqn.Append(ui.DOScale(Vector3.zero, 0.25F)).onComplete += () =>
            {
                ui.gameObject.SetActive(false);
            //InputReader.Instance.EnableGameplayInput();
        };
        }
    }
}
