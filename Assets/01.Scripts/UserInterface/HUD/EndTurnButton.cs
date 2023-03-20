using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace Penwyn.Game
{
    public class EndTurnButton : Button
    {
        protected override void Awake()
        {
            base.Awake();
            onClick.AddListener(() => { DuelManager.Instance.EndTurn(); });
        }
    }

}