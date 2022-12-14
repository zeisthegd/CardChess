using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Tools;
namespace Penwyn.Game
{
    [CreateAssetMenu(menuName = "Player/Data/DuelistData")]
    public class DuelistData : ScriptableObject
    {
        [Header("Deck")]
        public List<CardData> Deck;
        [Header("Energy")]
        public IntValue Energy = new IntValue(3, 3);

        public virtual DuelistData Clone()
        {
            DuelistData clone = Instantiate(this);
            clone.Deck.AddRange(this.Deck);
            return clone;
        }

        private void OnEnable()
        {
            Energy.Reset();
        }

        private void OnDisable()
        {

        }
    }
}
