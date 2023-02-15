using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using DG.Tweening;
using System;

using Penwyn.Tools;

namespace Penwyn.Game
{
    public class CardPlayingAnimationManager : SingletonMonoBehaviour<CardPlayingAnimationManager>
    {
        public float DragTime;
        private Sequence _mainSequence;
        private Card _currentCard;
        private bool _enabledFunctions = true;
        void OnEnable()
        {
            ConnectEvents();
        }
        /// <summary>
        /// When mouse left is clicked, take the pressed card for dragging.
        /// </summary>
        public void AcceptClick(Card card)
        {
            if (_enabledFunctions)
            {
                if (_currentCard != null)
                {
                    CancelClick();
                }
                CardHandAnimationController.Instance.DisableFunctions();
                _currentCard = card;
                MoveChosenCardToCenterOfHand();
                ProtagonistEventList.Instance.CardChosen.RaiseEvent(_currentCard);
            }
        }

        /// <summary>
        /// When mouse right is clicked, release the card to its position.
        /// </summary>
        public void CancelClick()
        {
            if (_currentCard != null && !DeckManager.Instance.IsDiscarded(_currentCard))// && CombatManager.Instance.CombatStarted)
            {
                CursorManager.Instance.ResetCursor();
                _currentCard.transform.DOKill();
                _currentCard.transform.SetParent(DeckManager.Instance.HandPile.transform);
                _currentCard = null;

                CardHandAnimationController.Instance.EnableFunctions();
                CardHandAnimationController.Instance.UpdateCardsTransform();
                ProtagonistEventList.Instance.ReleaseCard.RaiseEvent();
            }
        }

        /// <summary>
        /// Play discard card animation.
        /// </summary>
        public void PlayDiscardAnimation(Card card)
        {
            if (card != null)
            {
                card.gameObject.SetActive(true);
                card.DOKill();
                _currentCard = null;
                PlayAddCard(card, DeckManager.Instance.DiscardPileBtn, true);
            }
        }

        /// <summary>
        /// Play shuffle card animation.
        /// </summary>
        public void PlayShuffleAnimation(Card card)
        {
            PlayAddCard(card, DeckManager.Instance.DrawPileBtn, true);
        }

        /// <summary>
        /// Set up the card before moving it.
        /// Use the add sequence to add it to the pile.
        /// </summary>
        public void PlayAddCard(Card card, Transform desination, bool disableAfterAdd = false)
        {
            if (card != null)
            {
                card.gameObject.SetActive(true);
                card.transform.SetParent(desination);
                card.transform.DOKill();
                card.transform.localScale = Vector3.one;

                _currentCard = null;

                Sequence addCardSqn = PlayCardToCenterOfScreen(card);
                addCardSqn.Append(card.transform.DOMove(desination.position, DragTime));
                addCardSqn = DisableAfterAdd(addCardSqn, card.transform, disableAfterAdd);
                addCardSqn.onComplete += () =>
                {
                    CardHandAnimationController.Instance.UpdateCardsTransform();
                    CardHandAnimationController.Instance.EnableFunctions();
                };
            }
        }

        /// <summary>
        /// Move the card to the center of the screen.
        /// </summary>
        public Sequence PlayCardToCenterOfScreen(Card card)
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2F, Screen.height / 2F, transform.position.z);
            screenCenter = Camera.main.ScreenToWorldPoint(screenCenter);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(card.transform.DOMove(screenCenter, DragTime));
            sequence.Append(card.transform.DOMove(screenCenter, DragTime));
            sequence.Append(card.transform.DORotate(Vector3.zero, DragTime));
            return sequence;
        }



        /// <summary>
        /// If the card need to have a target to be used, move it to the center of the hand to make finding target easier.
        /// </summary>
        private void MoveChosenCardToCenterOfHand()
        {
            if (_currentCard != null)
            {
                Card chosenCard = _currentCard.GetComponent<Card>();
                chosenCard.transform.DOKill();
                Transform chosenCardZone = CardHandAnimationController.Instance.ChosenCardZone;
                chosenCard.transform.SetParent(chosenCardZone);
                chosenCard.transform.DOMove(chosenCardZone.position, DragTime * 2);
            }
        }

        private Sequence DisableAfterAdd(Sequence sequence, Transform transform, bool disable)
        {
            if (disable)
                sequence.Join(transform.DOScale(Vector3.zero, DragTime)).onComplete += () => { transform.gameObject.SetActive(false); };
            return sequence;
        }


        public void ConnectEvents()
        {
            InputReader.Instance.OnCancelStarted += CancelClick;
            GameEventList.Instance.CombatStart.OnEventRaised += EnableFunctions;
            GameEventList.Instance.ProtagonistWon.OnEventRaised += DisableFunctions;
            GameEventList.Instance.ProtagonistLost.OnEventRaised += DisableFunctions;
            CardEventList.Instance.PointerSelect.OnEventRaised += AcceptClick;

        }

        public void DisconnectEvents()
        {
            InputReader.Instance.OnCancelStarted -= CancelClick;
            GameEventList.Instance.CombatStart.OnEventRaised -= EnableFunctions;
            GameEventList.Instance.ProtagonistWon.OnEventRaised -= DisableFunctions;
            GameEventList.Instance.ProtagonistLost.OnEventRaised -= DisableFunctions;
            CardEventList.Instance.PointerSelect.OnEventRaised -= AcceptClick;
        }

        public void EnableFunctions()
        {
            _enabledFunctions = true;
        }

        public void DisableFunctions()
        {
            _enabledFunctions = false;
        }


        void OnDisable()
        {
            DisconnectEvents();
            DisableFunctions();
        }



    }

}
