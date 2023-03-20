using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using DG.Tweening;

using Penwyn.Tools;

namespace Penwyn.Game
{
    public class EndGameUI : MonoBehaviour
    {
        public TMP_Text WhiteSquareCountTxt;
        public TMP_Text BlackSquareCountTxt;
        public TMP_Text ResultOperator;
        public Button QuitRoomBtn;
        public Button RestartMatchBtn;

        private void Awake()
        {
            GetComponent<Canvas>().sortingOrder = 999;
        }

        public void PlayGameEndAnimation(int whiteCount, int blackCount)
        {
            int tempWhite = 0;
            int tempBlack = 0;

            DOTween.To(() => tempWhite, x => tempWhite = x, whiteCount, 3).OnUpdate(() => { WhiteSquareCountTxt.SetText(tempWhite.ToString()); });
            DOTween.To(() => tempBlack, x => tempBlack = x, blackCount, 3).OnUpdate(() => { BlackSquareCountTxt.SetText(tempBlack.ToString()); }).onComplete += () => { ShakeWinner(whiteCount, blackCount); }; ;
        }

        private void ShakeWinner(int whiteCount, int blackCount)
        {
            if (whiteCount > blackCount)
            {
                WhiteSquareCountTxt.transform.DOShakeScale(1, 0.5F, 3, 0).SetLoops(-1);
                WhiteSquareCountTxt.transform.DOShakePosition(1);
                WhiteSquareCountTxt.transform.DOShakeRotation(1).onComplete += () => { AnnounceResultOperator(">"); }; ;

            }
            else if (blackCount > whiteCount)
            {
                BlackSquareCountTxt.transform.DOShakeScale(1, 0.5F, 3, 0).SetLoops(-1);
                BlackSquareCountTxt.transform.DOShakePosition(1);
                BlackSquareCountTxt.transform.DOShakeRotation(1).onComplete += () => { AnnounceResultOperator("<"); };

            }
            else
            {
                AnnounceResultOperator("=");
            }
        }

        private void AnnounceResultOperator(string value)
        {
            ResultOperator.SetText(value);
            ResultOperator.transform.DOShakeScale(1, 0.5F, 3, 0).SetLoops(-1);
            ResultOperator.transform.DOShakePosition(1);
            ResultOperator.transform.DOShakeRotation(1);
        }

        private void OnDisable()
        {
            WhiteSquareCountTxt.transform.DOKill();
            BlackSquareCountTxt.transform.DOKill();
            ResultOperator.transform.DOKill();
        }
    }

}
