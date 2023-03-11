using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Tools;

namespace Penwyn.Game
{
    [CreateAssetMenu(menuName = "Game/Card/Action/Draw Cards")]
    public class DrawCardAction : Action
    {
        private void OnEnable()
        {
            Range = ActionRange.AUTO;
            _requiredChosenSquareSameColor = false;
            _requiredChosenPieceSameColor = false;
        }

        public override void Act()
        {
            base.Act();
            DuelManager.Instance.MasterDM.DrawCards(ActionValue.CurrentValue);
        }

        public override string GetDescription()
        {
            return "";
        }

        public override DisplayInfo GetInfo()
        {
            return new DisplayInfo();
        }
    }

}




