using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace Penwyn.Game
{
    public class SliderTMP : Slider
    {
        public TMP_Text ValueTxt;

        protected override void Update()
        {
            base.Update();
            if (ValueTxt)
                ValueTxt.text = value + "";
        }
    }

}