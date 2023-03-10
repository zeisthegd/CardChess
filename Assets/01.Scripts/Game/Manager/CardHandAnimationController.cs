using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using NaughtyAttributes;

using Penwyn.Tools;
namespace Penwyn.Game
{
    public class CardHandAnimationController : MonoBehaviour
    {
        [Header("Offsets")]
        public float PositionXOffset;
        public float PositionYOffset;
        public float RotateOffset;
        public bool LeftToRight = true;

        [Header("Hover")]
        public float HoverSpeed;
        public float HoverScale;
        public int NearbyCardPushCount = 1;//1 each for left and right.
        public float PushOffsetX = 1F;
        [Header("Objects")]
        public Transform ChosenCardZone;
        public Transform Hand;

        private float _scaleToHand = 1;
        private bool _enabledFunctions = true;
        private DeckManager _deckManager;

        void OnEnable()
        {
            _deckManager = GetComponent<DeckManager>();
            ConnectEvents();
        }

        /// <summary>
        /// Update cards transform and rotation
        /// </summary>
        [Button]
        public void UpdateCardsTransform()
        {
            for (int i = 0; i < _deckManager.HandPile.Count; i++)
            {
                Card card = _deckManager.HandPile.GetCard(i);
                card.gameObject.SetActive(true);
                Rescale(card);
                MoveToPosition(card, i);
                RotateAtPosition(card, i);
            }
            ReorderCards();
        }

        /// <summary>
        /// Rescale card so that the Hand can be fully visible even if it is full (10 cards).
        /// </summary>
        public void Rescale(Card card)
        {
            _scaleToHand = 6 / _deckManager.HandPile.Count;
            _scaleToHand = Mathf.Clamp(_scaleToHand, 0, 1);
            card.transform.DOScale(Vector3.one * _scaleToHand, HoverSpeed);
        }

        /// <summary>
        /// Tweening card's position
        /// </summary>
        public void MoveToPosition(Card card, int index)
        {
            Vector3 localPos = IndexToLocalPosition(index);
            card.transform.DOLocalMove(localPos, HoverSpeed);
            card.transform.SetParent(Hand);
        }

        /// <summary>
        /// Tweening card's rotation
        /// </summary>
        public void RotateAtPosition(Card card, int index)
        {
            Vector3 localRotation = IndexToLocalRotation(index);
            card.transform.DORotate(localRotation, HoverSpeed);
        }

        /// <summary>
        /// Play the hover animation if the card is on the handpile of this deck.
        /// And if there's no other card currently being hovered.
        /// </summary>
        private void Hover(Card card)
        {
            //Debug.Log("Hover: " + _deckManager.HandPile.Cards.Contains(card));
            if (CanHover(card))
            {
                var localPos = IndexToLocalPosition(_deckManager.HandPile.GetCardIndex(card));
                Vector3 destination = new Vector3(localPos.x, 0, localPos.z);

                card.transform.DOKill();
                card.transform.SetParent(ChosenCardZone);
                card.SetCanvasOrder(9999);
                card.transform.DOScale(Vector3.one * HoverScale, HoverSpeed / 2);
                card.transform.DOLocalMove(destination, HoverSpeed);
                card.transform.DORotate(Vector3.zero, HoverSpeed);
                MoveNearbyCardAside(card);
            }
        }


        /// <summary>
        /// When a card is hovered above, push the nearby cards away from it a little bit.
        /// </summary>
        private void MoveNearbyCardAside(Card card)
        {
            int index = _deckManager.HandPile.GetCardIndex(card);
            for (int i = 1; i <= NearbyCardPushCount; i++)
            {
                if (index + i < _deckManager.HandPile.Count)
                {
                    _deckManager.HandPile.GetCard(index + i).transform.DOLocalMoveX(IndexToLocalPosition(index + i).x + PushOffsetX / i, 0.25F);
                }
                if (index - i >= 0)
                {
                    _deckManager.HandPile.GetCard(index - i).transform.DOLocalMoveX(IndexToLocalPosition(index - i).x - PushOffsetX / i, 0.25F);
                }
            }
        }

        /// <summary>
        /// Move the card down when it is unhovered.
        /// </summary>
        private void Exit(Card card)
        {
            //Debug.Log("Exit: " + _deckManager.HandPile.Cards.Contains(card));
            if (CanExit(card))
            {
                card.transform.DOKill();
                Vector3 destination = new Vector3(card.transform.localPosition.x, 0, card.transform.localPosition.z);

                card.transform.DOLocalMove(destination, HoverSpeed);
                card.transform.SetParent(_deckManager.HandPile.transform);

                UpdateCardsTransform();
            }
        }

        /// <summary>
        /// Returns true if their is a card in the play zone.
        /// </summary>
        private bool ChosenCardZoneOccupied()
        {
            return ChosenCardZone.transform.childCount > 0;
        }

        public void ReorderCards()
        {
            for (int i = 0; i < _deckManager.HandPile.Count; i++)
            {
                _deckManager.HandPile.GetCard(i).transform.SetSiblingIndex(i);
                _deckManager.HandPile.GetCard(i).SetCanvasOrder(i);
            }
        }

        /// <summary>
        /// Get the adjusted position from index.
        /// </summary>
        private Vector3 IndexToLocalPosition(int index)
        {
            float difference = LeftToRight ? (float)index - 0 : -(index);//Increase from left to right
            Vector3 handLocalPos = _deckManager.HandPile.GetComponent<RectTransform>().anchoredPosition;
            Vector3 localPos = new Vector3((difference * PositionXOffset * _scaleToHand), -(Mathf.Abs((difference * PositionYOffset))), 0);
            return localPos;
        }

        /// <summary>
        /// Get the adjusted rotation from index.
        /// </summary>
        private Vector3 IndexToLocalRotation(int index)
        {
            float difference = (float)index - _deckManager.HandPile.HalfCount;
            Vector3 localRotation = new Vector3(Vector3.zero.x, Vector3.zero.y, Vector3.zero.z + (-difference * RotateOffset));
            return localRotation;
        }

        /// <summary>
        /// Check if mouse can hover above this card.
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        private bool CanHover(Card card)
        {
            return _enabledFunctions && !ChosenCardZoneOccupied() && !_deckManager.IsDiscarded(card) && _deckManager.HandPile.Cards.Contains(card);
        }

        /// <summary>
        /// Check if mouse can exit hover state of this card.
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        private bool CanExit(Card card)
        {
            return _enabledFunctions && !_deckManager.IsDiscarded(card) && _deckManager.HandPile.Cards.Contains(card);
        }

        public void ConnectEvents()
        {
            GameEventList.Instance.MatchStarted.OnEventRaised += EnableFunctions;
            GameEventList.Instance.ProtagonistWon.OnEventRaised += DisableFunctions;
            GameEventList.Instance.ProtagonistLost.OnEventRaised += DisableFunctions;
            CardEventList.Instance.PointerEnter.OnEventRaised += Hover;
            CardEventList.Instance.PointerExit.OnEventRaised += Exit;

        }

        public void DisconnectEvents()
        {
            GameEventList.Instance.MatchStarted.OnEventRaised -= EnableFunctions;
            GameEventList.Instance.ProtagonistWon.OnEventRaised -= DisableFunctions;
            GameEventList.Instance.ProtagonistLost.OnEventRaised -= DisableFunctions;
            CardEventList.Instance.PointerEnter.OnEventRaised -= Hover;
            CardEventList.Instance.PointerExit.OnEventRaised -= Exit;

        }

        public void EnableFunctions()
        {
            Debug.Log(gameObject.name + ": EnableFunctions");
            _enabledFunctions = true;
        }

        public void DisableFunctions()
        {
            Debug.Log(gameObject.name + ": DisableFunctions");

            _enabledFunctions = false;
        }

        private void OnDisable()
        {
            DisconnectEvents();
            DisableFunctions();
        }
    }

}
