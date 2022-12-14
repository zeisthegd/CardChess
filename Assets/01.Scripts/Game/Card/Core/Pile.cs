using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    /// <summary>
    /// Pile of _cards
    /// </summary>
    public class Pile : MonoBehaviour
    {
        public Transform PositionInCanvas;
        private PileView _pileView;
        private List<Card> _cards;


        private void Awake()
        {
            _pileView = GetComponent<PileView>();
            _cards = new List<Card>();
        }

        public Card GetCard(int index)
        {
            return _cards[index];
        }

        /// <summary>
        /// Add the card to the pile.
        /// </summary>
        public void Add(Card card)
        {
            if (!_cards.Contains(card))
            {
                _cards.Add(card);
            }
        }

        /// <summary>
        /// Remove the card from to the pile.
        /// </summary>
        public void Remove(Card card)
        {
            if (_cards.Contains(card))
            {
                _cards.Remove(card);
            }
        }

        public int GetCardIndex(Card card)
        {
            return _cards.IndexOf(card);
        }

        public float Count { get => _cards.Count; }
        public float HalfCount { get => (float)((_cards.Count - 1) / 2F); }

        public List<Card> Cards { get => _cards; }

        public PileView View { get => _pileView; }
    }

}

