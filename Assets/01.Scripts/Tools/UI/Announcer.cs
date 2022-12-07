using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;

namespace Penwyn.Tools
{
    public class Announcer : SingletonMonoBehaviour<Announcer>
    {
        [SerializeField][Range(0, 2)] float announceDuration = 0;
        [SerializeField] TMP_Text announceTxtPref;

        private List<TMP_Text> announceTxtList = new List<TMP_Text>();
        public void Announce(string text, bool toUpper = false, float duration = 0)
        {
            duration = Mathf.Max(duration, announceDuration);
            text = toUpper ? text.ToUpper() : text;
            for (int i = 0; i < announceTxtList.Count; i++)
            {
                if (announceTxtList[i] != null)
                {
                    announceTxtList[i].transform.DOScale(Vector3.one * 0.8F, 0.1F);
                    announceTxtList[i].transform.DOLocalMoveY(announceTxtList[i].transform.localPosition.y + announceTxtPref.rectTransform.sizeDelta.y, 0.1F);
                }
            }

            Canvas rootCanvas = FindObjectOfType<Canvas>().rootCanvas;
            TMP_Text newAnncText = Instantiate(announceTxtPref, rootCanvas.transform.position + (Vector3.up * Screen.height * 0.375F), Quaternion.identity, rootCanvas.transform);

            announceTxtList.Add(newAnncText);
            newAnncText.text = text;
            newAnncText.DOFade(1, duration).onComplete += () =>
            {
                announceTxtList.Remove(newAnncText);
                Destroy(newAnncText.gameObject);
            };
        }

        public void ClearTexts()
        {
            for (int i = 0; i < announceTxtList.Count; i++)
            {
                if (announceTxtList[i] != null)
                {
                    announceTxtList[i].DOKill();
                    Destroy(announceTxtList[i].gameObject);
                }
            }
        }
    }
}
