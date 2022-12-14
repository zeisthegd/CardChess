using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    public class Protagonist : Duelist
    {
        public override void CardUsed(Card card)
        {
            base.CardUsed(card);
            ProtagonistEventList.Instance.EnergyAltered.RaiseEvent();
        }

        
        private void OnEnable()
        {
            ConnectEvents();
        }

        private void OnDisable()
        {
            DisonnectEvents();
        }
    }
}

