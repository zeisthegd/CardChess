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
        private bool _canPlay = false;
        private DeckManager _deckManager;

        void OnEnable()
        {
            _deckManager = GetComponent<DeckManager>();
            ConnectEvents();
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
                InitiateCardChosenSequence(card);
            }
            else
            {
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
            Debug.Log("ChosenCardDonePlaying");
            if (card != null)//&& have target
            {
                CursorManager.Instance.ResetCursor();
                UntargetAll();
                _deckManager.Discard(card);
                card.Owner.Data.Energy.SetCurrentValue(card.Owner.Data.Energy.CurrentValue - card.Data.Cost.CurrentValue);

                _canPlay = false;
                _chosenCard = null;

                _deckManager.CardHandAnimationController.EnableFunctions();
                _deckManager.CardHandAnimationController.UpdateCardsTransform();

                EndTurnIfCardIsDeployCat(card);
            }
        }

        private void EndTurnIfCardIsDeployCat(Card card)
        {
            DuelManager.Instance.EndTurn();
        }



        /// <summary>
        /// Cancel the chosen card.
        /// </summary>
        public void CancelCard()
        {
            if (_targetList != null)
                UntargetAll();
            _chosenCard = null;
            _canPlay = false;
            _deckManager.CardActionHandler.EndCurrentAction(false);
            _deckManager.CardPlayingAnimationManager.CancelClick();
        }

        /// <summary>
        /// Untarget and clear the _targetList.
        /// </summary>
        private void UntargetAll()
        {
            foreach (Piece target in _targetList)
                Untarget(target);
            _targetList.Clear();
        }

        /// <summary>
        /// Release a target.
        /// </summary>
        public void Untarget(Piece unit)
        {
            if (unit != null)
            {
                //unit.GetView().TargetingUI.Exit();
                _canPlay = false;
            }
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
