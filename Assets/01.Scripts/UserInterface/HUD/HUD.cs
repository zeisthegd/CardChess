using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;
using TMPro;

using Penwyn.Game;
using Penwyn.Tools;

namespace Penwyn.UI
{
    public class HUD : SingletonMonoBehaviour<HUD>
    {

        public Button EndTurnButton;
        public TMP_Text EnergyTxt;

        protected virtual void OnEnable()
        {
            ConnectEvents();
        }

        protected virtual void OnDisable()
        {
            DisconnectEvents();
        }

        public virtual void ConnectEvents()
        {
            EndTurnButton?.onClick.AddListener(() => { DuelManager.Instance.EndTurn(); });
        }

        public virtual void DisconnectEvents()
        {
            EndTurnButton?.onClick.RemoveAllListeners();
        }
    }
}

