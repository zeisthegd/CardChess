using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    public class Duelist : MonoBehaviour
    {
        public DuelistData Data;

        public virtual void CardUsed(Card card)
        {
            Data.Energy.CurrentValue -= card.Data.Cost.CurrentValue;
        }

        public void ConnectEvents()
        {
            CardEventList.Instance.CardUsed.OnEventRaised += CardUsed;
        }

        public void DisonnectEvents()
        {
            CardEventList.Instance.CardUsed.OnEventRaised -= CardUsed;
        }

    }

}