using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    public class Duelist : MonoBehaviour
    {
        private DuelistData _data;
        private Faction _faction;

        public void Load(DuelistData data, Faction faction)
        {
            this._data = data;
            _faction = faction;
        }

        public virtual void CardUsed(Card card)
        {
            _data.Energy.CurrentValue -= card.Data.Cost.CurrentValue;
        }


        public void ConnectEvents()
        {
            CardEventList.Instance.CardUsed.OnEventRaised += CardUsed;
        }

        public void DisonnectEvents()
        {
            CardEventList.Instance.CardUsed.OnEventRaised -= CardUsed;
        }
        
        public Faction Faction { get => _faction; }
        public DuelistData Data { get => _data; }
    }

}