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
    public class CardPlayer : SingletonMonoBehaviour<CardPlayer>
    {
        private List<Piece> _targetList = new List<Piece>();
        private Card _chosenCard = null;
        private bool _canPlay = false;


        void OnEnable()
        {
            ConnectEvents();
        }

        /// <summary>
        /// Play the chosen card.
        /// Print description for now.
        /// </summary>
        public void PlayChosenCard()
        {
            if (_chosenCard != null)//&& have target
            {
                if (_chosenCard.EnoughEnergy)
                {
                    _chosenCard.Use(_targetList);
                    CursorManager.Instance.ResetCursor();
                    UntargetAll();
                    Play(_chosenCard);
                    _canPlay = false;
                    _chosenCard = null;
                    CardHandAnimationController.Instance.EnableFunctions();
                    CardHandAnimationController.Instance.UpdateCardsTransform();
                }
                else
                {
                    CardPlayingAnimationManager.Instance.CancelClick();
                }
            }
        }
        public void Play(Card card)
        {

        }

        /// <summary>
        /// Choose the card to play.
        /// </summary>
        public void Choose(Card card)
        {
            _chosenCard = card;
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
        /// Target an enemy, add it to the list, show the target UI.
        /// </summary>
        public void TargetEnemy(Piece piece)
        {
        }

        /// <summary>
        /// Untarget all enemies.
        /// </summary>
        public void UntargetEnemy(Piece piece)
        {
            if (_chosenCard != null)
            {
                UntargetAll();
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
        /// Target a player.
        /// </summary>
        public void Target(Piece player)
        {
            if (_chosenCard != null)
            {
                if (!_targetList.Contains(player))
                {
                    _targetList.Add(player);
                    //player.GetView().TargetingUI.Target();
                }
                _canPlay = true;
            }
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
            InputReader.Instance.OnChooseCancelled += PlayChosenCard;

            ProtagonistEventList.Instance.CardChosen.OnEventRaised += Choose;
            ProtagonistEventList.Instance.ReleaseCard.OnEventRaised += CancelCard;

            ProtagonistEventList.Instance.MouseEnteredPlayZone.OnEventRaised += MouseEnteredPlayZone;
            ProtagonistEventList.Instance.MouseExitedPlayZone.OnEventRaised += MouseExitedPlayZone;

        }

        void DisonnectEvents()
        {
            InputReader.Instance.OnChooseCancelled -= PlayChosenCard;

            ProtagonistEventList.Instance.CardChosen.OnEventRaised -= Choose;
            ProtagonistEventList.Instance.ReleaseCard.OnEventRaised -= CancelCard;

            ProtagonistEventList.Instance.MouseEnteredPlayZone.OnEventRaised -= MouseEnteredPlayZone;
            ProtagonistEventList.Instance.MouseExitedPlayZone.OnEventRaised -= MouseExitedPlayZone;

        }
        public Card ChosenCard { get => _chosenCard; }
        public List<Piece> TargetList { get => _targetList; }
    }

}
