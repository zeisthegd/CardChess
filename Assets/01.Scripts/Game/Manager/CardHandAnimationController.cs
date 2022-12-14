using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using NaughtyAttributes;

using Penwyn.Tools;
namespace Penwyn.Game
{
    public class CardHandAnimationController : SingletonMonoBehaviour<CardHandAnimationController>
    {
        [Header("Offsets")]
        public float PositionXOffset;
        public float PositionYOffset;
        public float RotateOffset;

        [Header("Hover")]
        public float HoverSpeed;
        public float HoverScale;
        public int NearbyCardPushCount = 1;//1 each for left and right.
        public float PushOffsetX = 1F;
        [Header("Objects")]
        public Transform ChosenCardZone;
        public Transform Hand;

        private float _scaleToHand = 1;

        void OnEnable()
        {
            ConnectEvents();
        }

        /// <summary>
        /// Update cards transform and rotation
        /// </summary>
        [Button]
        public void UpdateCardsTransform()
        {
            for (int i = 0; i < DeckManager.Instance.HandPile.Count; i++)
            {
                Card card = DeckManager.Instance.HandPile.GetCard(i);
                card.gameObject.SetActive(true);
                //Rescale(card);
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
            _scaleToHand = 6 / DeckManager.Instance.HandPile.Count;
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
        /// Move the card up when it is hovered.
        /// </summary>
        private void Hover(Card card)
        {
            //Debug.Log("Hover: " + DeckManager.Instance.HandPile.Cards.Contains(card));
            if (!ChosenCardZoneOccupied() && !DeckManager.Instance.IsDiscarded(card) && DeckManager.Instance.HandPile.Cards.Contains(card))
            {
                var localPos = IndexToLocalPosition(DeckManager.Instance.HandPile.GetCardIndex(card));
                Vector3 destination = new Vector3(localPos.x, 0, localPos.z);

                card.transform.DOKill();
                card.transform.SetParent(ChosenCardZone);
                card.SetCanvasOrder(9999);
                card.transform.DOScale(Vector3.one * HoverScale, HoverSpeed / 2);
                card.transform.DOLocalMove(destination, HoverSpeed);
                card.transform.DORotate(Vector3.zero, HoverSpeed).onComplete += card.ShowInfo;
                MoveNearbyCardAside(card);
            }
        }


        /// <summary>
        /// When a card is hovered above, push the nearby cards away from it a little bit.
        /// </summary>
        private void MoveNearbyCardAside(Card card)
        {
            if (true)
            {
                int index = DeckManager.Instance.HandPile.GetCardIndex(card);
                for (int i = 1; i <= NearbyCardPushCount; i++)
                {
                    if (index + i < DeckManager.Instance.HandPile.Count)
                    {
                        DeckManager.Instance.HandPile.GetCard(index + i).transform.DOLocalMoveX(IndexToLocalPosition(index + i).x + PushOffsetX / i, 0.25F);
                    }
                    if (index - i >= 0)
                    {
                        DeckManager.Instance.HandPile.GetCard(index - i).transform.DOLocalMoveX(IndexToLocalPosition(index - i).x - PushOffsetX / i, 0.25F);
                    }
                }
            }
        }

        /// <summary>
        /// Move the card down when it is unhovered.
        /// </summary>
        private void Exit(Card card)
        {
            //Debug.Log("Exit: " + DeckManager.Instance.HandPile.Cards.Contains(card));

            if (!DeckManager.Instance.IsDiscarded(card) && DeckManager.Instance.HandPile.Cards.Contains(card))
            {
                card.transform.DOKill();
                Vector3 destination = new Vector3(card.transform.localPosition.x, 0, card.transform.localPosition.z);

                card.transform.DOLocalMove(destination, HoverSpeed);
                card.transform.SetParent(DeckManager.Instance.HandPile.transform);

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
            for (int i = 0; i < DeckManager.Instance.HandPile.Count; i++)
            {
                DeckManager.Instance.HandPile.GetCard(i).transform.SetSiblingIndex(i);
                DeckManager.Instance.HandPile.GetCard(i).SetCanvasOrder(i);
            }
        }

        /// <summary>
        /// Get the adjusted position from index.
        /// </summary>
        private Vector3 IndexToLocalPosition(int index)
        {
            float difference = (float)index - DeckManager.Instance.HandPile.HalfCount;
            Vector3 handLocalPos = DeckManager.Instance.HandPile.transform.localPosition;
            Vector3 localPos = new Vector3(handLocalPos.x + (difference * PositionXOffset * _scaleToHand), -(Mathf.Abs((difference * PositionYOffset))), 0);
            return localPos;
        }

        /// <summary>
        /// Get the adjusted rotation from index.
        /// </summary>
        private Vector3 IndexToLocalRotation(int index)
        {
            float difference = (float)index - DeckManager.Instance.HandPile.HalfCount;
            Vector3 localRotation = new Vector3(Vector3.zero.x, Vector3.zero.y, Vector3.zero.z + (-difference * RotateOffset));
            return localRotation;
        }

        public void ConnectEvents()
        {
            GameEventList.Instance.CombatStart.OnEventRaised += EnableFunctions;
            GameEventList.Instance.ProtagonistWon.OnEventRaised += DisableFunctions;
            GameEventList.Instance.ProtagonistLost.OnEventRaised += DisableFunctions;
        }

        public void DisconnectEvents()
        {
            GameEventList.Instance.CombatStart.OnEventRaised -= EnableFunctions;
            GameEventList.Instance.ProtagonistWon.OnEventRaised -= DisableFunctions;
            GameEventList.Instance.ProtagonistLost.OnEventRaised -= DisableFunctions;
        }

        public void EnableFunctions()
        {
            CardEventList.Instance.PointerEnter.OnEventRaised += Hover;
            CardEventList.Instance.PointerExit.OnEventRaised += Exit;
        }

        public void DisableFunctions()
        {
            CardEventList.Instance.PointerEnter.OnEventRaised -= Hover;
            CardEventList.Instance.PointerExit.OnEventRaised -= Exit;
        }

        private void OnDisable()
        {
            DisconnectEvents();
            DisableFunctions();
        }
    }

}
