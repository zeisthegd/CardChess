using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

using TMPro;


namespace Penwyn.Game
{
    public class PileView : MonoBehaviour
    {
        [Header("View")]
        public GameObject View;
        public RectTransform Container;

        [Header("Texts")]
        public TMP_Text Count;


        [Header("Animation Settings")]
        public float HighLightSize;
        public float DisplaySize;
        public float Duration;
        private bool _isOpened = false;
        private Pile _pile;
        List<Card> clones = new List<Card>();


        void Start()
        {
            _pile = GetComponent<Pile>();
            ConnectGeneralEvents();
        }
        public void Act()
        {
            CloseOtherViews();
            if (View.activeInHierarchy == true)
                CloseDisplay();
            else DisplayCards();
        }


        /// <summary>
        /// Open the _pile View and display card.
        /// </summary>
        public void DisplayCards()
        {
            _isOpened = true;
            View.SetActive(_isOpened);
            CardHandAnimationController.Instance.DisableFunctions();
            CardPlayingAnimationManager.Instance.DisableFunctions();
            ConnectEvents();
            foreach (Card card in _pile.Cards)
            {
                Card clone = Instantiate(card);
                clone.transform.DOComplete();
                clone.transform.SetParent(Container);
                clone.gameObject.SetActive(true);
                clone.DisplayCard(card.Data);
                clone.transform.DOScale(DisplaySize, 0);
                clone.transform.DORotate(Vector3.zero, 0);
                clones.Add(clone);
            }
        }

        /// <summary>
        /// Close the _pile View.
        /// </summary>
        public void CloseDisplay()
        {
            _isOpened = false;
            View.SetActive(_isOpened);
            CardHandAnimationController.Instance.EnableFunctions();
            CardPlayingAnimationManager.Instance.EnableFunctions();
            DisconnectEvents();
            foreach (Card clone in clones)
            {
                Destroy(clone.gameObject);
            }
            clones.Clear();
        }

        void CloseOtherViews()
        {
            PileView[] otherPileViews = FindObjectsOfType<PileView>();
            foreach (PileView View in otherPileViews)
            {
                if (View != this && View._isOpened)
                    View.CloseDisplay();
            }
        }

        /// <summary>
        /// Highlight the card.
        /// </summary>
        public void HighLight(Card card)
        {
            //card.ShowInfo();
            card.transform.DOScale(HighLightSize, Duration);
            card.transform.DORotate(Vector3.zero, 0);
        }

        /// <summary>
        /// Unhightlight the card.
        /// </summary>
        public void Exit(Card card)
        {
            card.transform.DOScale(DisplaySize, Duration);
            card.transform.DORotate(Vector3.zero, 0);
        }

        /// <summary>
        /// Update the display text of cards count.
        /// </summary>
        public void UpdateCount(Card card)
        {
            if (Count != null)
                Count.text = _pile.Count.ToString();
        }
        public void ConnectEvents()
        {
            CardEventList.Instance.PointerEnter.OnEventRaised += HighLight;
            CardEventList.Instance.PointerExit.OnEventRaised += Exit;
        }

        public void DisconnectEvents()
        {
            CardEventList.Instance.PointerEnter.OnEventRaised -= HighLight;
            CardEventList.Instance.PointerExit.OnEventRaised -= Exit;
        }

        private void ConnectGeneralEvents()
        {
            CardEventList.Instance.CardDrawn.OnEventRaised += UpdateCount;
            CardEventList.Instance.CardDiscarded.OnEventRaised += UpdateCount;
        }

        private void DisconnectGeneralEvents()
        {
            CardEventList.Instance.CardDrawn.OnEventRaised -= UpdateCount;
            CardEventList.Instance.CardDiscarded.OnEventRaised -= UpdateCount;
        }

        void OnDisable()
        {
            DisconnectGeneralEvents();
        }
    }

}