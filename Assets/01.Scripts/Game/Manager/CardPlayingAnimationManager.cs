using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using DG.Tweening;

using Penwyn.Tools;

namespace Penwyn.Game
{
    public class CardPlayingAnimationManager : MonoBehaviour
    {
        public float DragTime;
        private Sequence _mainSequence;
        private Card _currentCard;
        private bool _enabledFunctions = true;
        private DeckManager _deckManager;
        void OnEnable()
        {
            _deckManager = GetComponent<DeckManager>();
            ConnectEvents();
        }

        /// <summary>
        /// When mouse left is clicked, take the pressed card for dragging.
        /// </summary>
        public void AcceptClick(Card card)
        {
            if (_enabledFunctions && _deckManager.HasCardInDeck(card))
            {
                if (_currentCard != null)
                {
                    CancelClick();
                }
                _deckManager.CardHandAnimationController.DisableFunctions();
                _currentCard = card;
                ProtagonistEventList.Instance.CardChosen.RaiseEvent(_currentCard);
            }
        }

        /// <summary>
        /// When mouse right is clicked, release the card to its position.
        /// </summary>
        public void CancelClick()
        {
            if (_currentCard != null && !_deckManager.IsDiscarded(_currentCard))
            {
                CursorManager.Instance.ResetCursor();
                _currentCard.transform.DOKill();
                _currentCard.transform.SetParent(_deckManager.HandPile.transform);
                _currentCard = null;

                _deckManager.CardHandAnimationController.EnableFunctions();
                _deckManager.CardHandAnimationController.UpdateCardsTransform();
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
                card.SetInfoVisibility(true);
                card.ShowNormal();
                card.gameObject.SetActive(true);
                card.DOKill();
                _currentCard = null;
                PlayAddCard(card, _deckManager.DiscardPileBtn, true);
            }
        }

        /// <summary>
        /// Play shuffle card animation.
        /// </summary>
        public void PlayShuffleAnimation(Card card)
        {
            PlayAddCard(card, _deckManager.DrawPileBtn, true);
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
                    _deckManager.CardHandAnimationController.UpdateCardsTransform();
                    _deckManager.CardHandAnimationController.EnableFunctions();
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
            RectTransform cardRect = card.GetComponent<RectTransform>();
            cardRect.SetParent(_deckManager.transform);
            sequence.Append(cardRect.DOLocalMove(screenCenter, DragTime));
            sequence.Append(cardRect.DOLocalMove(screenCenter, DragTime));
            sequence.Append(cardRect.DORotate(Vector3.zero, DragTime));
            return sequence;
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
            GameEventList.Instance.MatchStarted.OnEventRaised += EnableFunctions;
            CardEventList.Instance.PointerSelect.OnEventRaised += AcceptClick;

        }

        public void DisconnectEvents()
        {
            InputReader.Instance.OnCancelStarted -= CancelClick;
            GameEventList.Instance.MatchStarted.OnEventRaised -= EnableFunctions;
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
