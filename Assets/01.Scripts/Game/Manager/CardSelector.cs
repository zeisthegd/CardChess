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
    public class CardSelector : SingletonMonoBehaviour<CardSelector>
    {
        private List<Piece> _targetList = new List<Piece>();
        private Card _chosenCard = null;
        private bool _canPlay = false;


        void OnEnable()
        {
            ConnectEvents();
        }

        /// <summary>
        /// Choose the card to play.
        /// </summary>
        public void Choose(Card card)
        {
            if (_chosenCard != null)
            {
                CancelCard();
            }
            if (DuelManager.Instance.IsMainPlayerTurn)
            {
                if (card.EnoughEnergy)
                {
                    _chosenCard = card;
                    CardActionHandler.Instance.GenerateActionQueue(_chosenCard);
                    CardActionHandler.Instance.StartNextAction();
                    CardEventList.Instance.CardDonePlaying.OnEventRaised += ChosenCardDonePlaying;
                }
                else
                {
                    Announcer.Instance.Announce("No Energy.");
                }
            }
            else
            {
                Announcer.Instance.Announce("It is not your turn, Dummy.");
            }
        }

        /// <summary>
        /// Play the chosen card.
        /// Print description for now.
        /// </summary>
        public void ChosenCardDonePlaying(Card card)
        {
            if (card != null)//&& have target
            {
                CursorManager.Instance.ResetCursor();
                UntargetAll();
                DeckManager.Instance.Discard(card);
                card.Owner.Data.Energy.SetCurrentValue(card.Owner.Data.Energy.CurrentValue - card.Data.Cost.CurrentValue);

                _canPlay = false;
                _chosenCard = null;
                CardHandAnimationController.Instance.EnableFunctions();
                CardHandAnimationController.Instance.UpdateCardsTransform();
            }
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
            CardActionHandler.Instance.EndCurrentAction(false);
            CardPlayingAnimationManager.Instance.CancelClick();
        }

        /// <summary>
        /// If the card need target, the gamer has to choose one, else the target is the protagonist.
        /// </summary>
        public void MouseEnteredPlayZone()
        {
            bool cardChosen = _chosenCard != null;
            if (cardChosen)
            {
                if (!_chosenCard.EnoughEnergy)
                    CardPlayingAnimationManager.Instance.CancelClick();
            }
            bool targetChosen = _targetList.Count > 0;
            _canPlay = cardChosen && targetChosen;
        }

        /// <summary>
        /// Revoke the ability to play the card.
        /// </summary>
        public void MouseExitedPlayZone()
        {
            if (_chosenCard == null)
            {
                UntargetAll();
                _canPlay = false;
            }
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

            ProtagonistEventList.Instance.MouseEnteredPlayZone.OnEventRaised += MouseEnteredPlayZone;
            ProtagonistEventList.Instance.MouseExitedPlayZone.OnEventRaised += MouseExitedPlayZone;

        }

        void DisonnectEvents()
        {

            ProtagonistEventList.Instance.CardChosen.OnEventRaised -= Choose;
            ProtagonistEventList.Instance.ReleaseCard.OnEventRaised -= CancelCard;

            ProtagonistEventList.Instance.MouseEnteredPlayZone.OnEventRaised -= MouseEnteredPlayZone;
            ProtagonistEventList.Instance.MouseExitedPlayZone.OnEventRaised -= MouseExitedPlayZone;

        }
        public Card ChosenCard { get => _chosenCard; }
        public List<Piece> TargetList { get => _targetList; }
    }

}
