﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Penwyn.Tools;

namespace Penwyn.Game
{
    public class DeckManager : SingletonMonoBehaviour<DeckManager>
    {
        [Header("Card Prefabs")]
        public Card CardPrefab;

        [Header("Buttons")]
        public RectTransform DrawPileBtn;
        public RectTransform DiscardPileBtn;

        [Header("Deck")]
        public Pile DrawPile;
        public Pile DiscardPile;
        public Pile HandPile;
        public Pile AllCards;

        [Header("Settings")]
        public List<CardData> Deck;
        public IntValue StartAmount;

        void OnEnable()
        {
            ConnectEvents();
        }

        /// <summary>
        /// Create all card gameObjects
        /// </summary>
        public void InitializeCards()
        {
            foreach (CardData data in Deck)
            {
                AllCards.Add(AddNewCard(DrawPile, data, PlayerManager.Instance.MainPlayer));
            }
            AllCards.View.UpdateCount(AllCards.Cards[0]);
        }

        /// <summary>
        /// Add the card object and data to a pile.
        /// </summary>
        public Card AddNewCard(Pile pile, CardData data, Duelist owner, bool playAnimation = false)
        {
            var card = CreateCard(data);
            pile.Add(card);
            if (playAnimation)
                CardPlayingAnimationManager.Instance.PlayAddCard(card, pile.PositionInCanvas, pile != HandPile);
            card.Owner = owner;
            return card;
        }

        /// <summary>
        /// Create a card gameObject from a CardData.
        /// </summary>
        public Card CreateCard(CardData data, bool activeAfterCreation = false)
        {
            Card card = Instantiate(CardPrefab, DrawPileBtn.position, Quaternion.identity, this.transform);
            card.SetData(data.Clone());
            card.DisplayCard(card.Data);
            card.gameObject.SetActive(activeAfterCreation);
            return card;
        }

        /// <summary>
        /// Return all cards to the draw pile
        /// </summary>
        public void AllCardsToDrawPile()
        {
            HandPile.Cards.Clear();
            DiscardPile.Cards.Clear();
            DrawPile.Cards.Clear();
            DrawPile.Cards.AddRange(AllCards.Cards);
            foreach (Card card in DrawPile.Cards)
            {
                card.transform.position = DrawPile.PositionInCanvas.position;
                card.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Create a temporary card with no UI.
        /// </summary>
        public Card CreateTempCard(CardData data)
        {
            var tempCard = Instantiate(CardPrefab, DrawPileBtn.position, Quaternion.identity, this.transform);
            tempCard.SetData(data);
            return tempCard;
        }

        /// <summary>
        /// Draw a set number of cards turn start.
        /// </summary>
        public void DrawCardsAtTurnStart()
        {
            DrawCards(StartAmount.CurrentValue);
        }

        /// <summary>
        /// Draw a fixed number of cards.
        /// </summary>
        /// <param name="amount">Amount to draw.</param>
        public void DrawCards(int amount)
        {
            StartCoroutine(Draw(amount));
        }

        /// <summary>
        /// Draw a number of cards when the turn start.
        /// </summary>
        public IEnumerator Draw(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                if (DrawPile.Count <= 0)
                {
                    ReshuffleCards();
                    yield return new WaitForSeconds(1);
                }
                DrawRandomCard();
                if (HandPile.Count >= 10)
                {
                    Debug.Log("<color=red>Your hand is full of cards.</color>");
                    break;
                }
            }
            CardHandAnimationController.Instance.UpdateCardsTransform();
        }

        /// <summary>
        /// Draw a random card.
        /// </summary>
        public void DrawRandomCard()
        {
            int randomIndex = Random.Range(0, (int)DrawPile.Count - 1);

            Card card = DrawPile.GetCard(randomIndex);
            HandPile.Add(card);
            DrawPile.Remove(card);

            card.gameObject.SetActive(true);
            card.transform.position = DrawPileBtn.position;
            card.DisplayCard(card.Data);
            CardHandAnimationController.Instance.MoveToPosition(card, HandPile.GetCardIndex(card));
            CardEventList.Instance.CardDrawn.RaiseEvent(card);
        }

        /// <summary>
        /// Discard every cards in the hand.
        /// </summary>
        public void DiscardHand()
        {
            for (int i = 0; i < HandPile.Count; i++)
            {
                Discard(HandPile.Cards[i]);
            }
            if (HandPile.Count > 0)
                DiscardHand();
        }

        /// <summary>
        /// Change the card to the discard pile list and play the animation.
        /// </summary>
        public void Discard(Card card)
        {
            HandPile.Remove(card);
            DiscardPile.Add(card);
            card.transform.SetParent(DiscardPile.transform);

            CardPlayingAnimationManager.Instance.PlayDiscardAnimation(card);
            CardHandAnimationController.Instance.UpdateCardsTransform();

            CardEventList.Instance.CardDiscarded.RaiseEvent(card);
        }

        /// <summary>
        /// Shuffle the cards in the discard pile into the draw pile.
        /// </summary>
        public void ReshuffleCards()
        {
            for (int i = 0; i < DiscardPile.Count; i++)
            {
                Card card = DiscardPile.GetCard(i);
                card.transform.SetParent(DiscardPile.transform);

                DrawPile.Add(card);
                DiscardPile.Remove(card);

                CardPlayingAnimationManager.Instance.PlayShuffleAnimation(card);
                CardHandAnimationController.Instance.UpdateCardsTransform();
            }
            if (DiscardPile.Count > 0)
                ReshuffleCards();
        }

        /// <summary>
        /// Return true if a card is discarded.
        /// </summary>
        public bool IsDiscarded(Card card)
        {
            return DiscardPile.Cards.Contains(card);
        }

        void ConnectEvents()
        {
            GameEventList.Instance.ProtagonistWon.OnEventRaised += AllCardsToDrawPile;
        }

        void DisonnectEvents()
        {
            GameEventList.Instance.ProtagonistWon.OnEventRaised -= AllCardsToDrawPile;

        }

        void OnDisable()
        {
            DisonnectEvents();
        }
    }

}