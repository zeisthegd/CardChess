using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using Penwyn.Tools;

namespace Penwyn.Game
{
    /// <summary>
    /// Responsible for card selection, usage and target finding.
    /// </summary>
    public class CardSelector : MonoBehaviour
    {
        private List<Piece> _targetList = new List<Piece>();
        private Card _chosenCard = null;
        private DeckManager _deckManager;

        void OnEnable()
        {
            _deckManager = GetComponent<DeckManager>();
            ConnectEvents();
        }

        void OnDisable()
        {
            DisonnectEvents();
        }

        /// <summary>
        /// Choose the card to play.
        /// </summary>
        public void Choose(Card card)
        {
            if (_chosenCard != null)//Cancel currenly chosen card.
                CancelCard();
            if (DuelManager.Instance.GuestDM == this._deckManager)
            {
                if (this._deckManager.HasCardInDeck(card))
                    Announcer.Instance.Announce("That's your opponent's card");
                return;
            }
            if (DuelManager.Instance.IsMainPlayerTurn)
            {
                GameManager.Instance.AudioPlayer.PlayConfirmSfx();
                InitiateCardChosenSequence(card);
            }
            else
            {
                GameManager.Instance.AudioPlayer.PlayCancelSfx();
                Announcer.Instance.Announce("It is not your turn, Dummy.");
            }
        }

        private void InitiateCardChosenSequence(Card card)
        {
            if (card.EnoughEnergy)
            {
                CardEventList.Instance.CardDonePlaying.OnEventRaised += ChosenCardDonePlaying;
                _chosenCard = card;
                _deckManager.CardActionHandler.GenerateActionQueue(_chosenCard);
                _deckManager.CardActionHandler.StartNextAction();
            }
            else
            {
                Announcer.Instance.Announce("No Energy.");
            }
        }

        /// <summary>
        /// Play the chosen card.
        /// Print description for now.
        /// </summary>
        public void ChosenCardDonePlaying(Card card)
        {
            CardEventList.Instance.CardDonePlaying.OnEventRaised -= ChosenCardDonePlaying;
            if (_chosenCard != null)//&& have target
            {
                CursorManager.Instance.ResetCursor();
                _deckManager.Discard(card);
                card.Owner.Data.Energy.SetCurrentValue(card.Owner.Data.Energy.CurrentValue - card.Data.Cost.CurrentValue);

                _chosenCard = null;

                _deckManager.CardHandAnimationController.EnableFunctions();
                _deckManager.CardHandAnimationController.UpdateCardsTransform();
            }
        }



        /// <summary>
        /// Cancel the chosen card.
        /// </summary>
        public void CancelCard()
        {
            GameManager.Instance.AudioPlayer.PlayCancelSfx();
            _chosenCard = null;
            _deckManager.CardActionHandler.EndCurrentAction(false);
            _deckManager.CardPlayingAnimationManager.CancelClick();
        }

        void ConnectEvents()
        {

            ProtagonistEventList.Instance.CardChosen.OnEventRaised += Choose;
            ProtagonistEventList.Instance.ReleaseCard.OnEventRaised += CancelCard;

        }

        void DisonnectEvents()
        {

            ProtagonistEventList.Instance.CardChosen.OnEventRaised -= Choose;
            ProtagonistEventList.Instance.ReleaseCard.OnEventRaised -= CancelCard;
        }
        public Card ChosenCard { get => _chosenCard; }
        public List<Piece> TargetList { get => _targetList; }
    }

}
