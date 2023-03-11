using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Tools;

namespace Penwyn.Game
{
    public class CardActionHandler : MonoBehaviour
    {
        private Square _chosenSquare;
        private Piece _chosenPiece;
        private Action _currentAction;

        private Queue<Action> _actionQueue;
        private Card _currentCard;

        public void GenerateActionQueue(Card card)
        {
            _currentCard = card;
            _actionQueue = new Queue<Action>();
            foreach (Action action in card.Data.Actions)
            {
                _actionQueue.Enqueue(action);
                Debug.Log($"Action name: {action.name}");

            }
        }

        public void StartNextAction()
        {
            _currentAction = null;
            if (_actionQueue.Count > 0)
                _currentAction = _actionQueue.Dequeue();
            if (_currentAction != null)
                StartAction(_currentAction);
            else
            {
                CardEventList.Instance.CardDonePlaying.RaiseEvent(_currentCard);
                Debug.Log("No more action.");
            }
        }

        /// <summary>
        /// Start an action, announce needed information on the UI.
        /// Set the event trigger to OnSquareSelected.
        /// </summary>
        public void StartAction(Action action)
        {
            switch (action.Range)
            {
                case ActionRange.CHOOSE_SQUARE:
                    DuelManager.Instance.PhaseMachine.Change(Phase.CHOOSE_SQUARE);
                    Announcer.Instance.Announce("Please choose a square");
                    SquareEventList.Instance.SquareSelected.OnEventRaised += OnSquareSelected;
                    break;

                case ActionRange.CHOOSE_PIECE:
                    Announcer.Instance.Announce("Please choose a piece");
                    SquareEventList.Instance.SquareSelected.OnEventRaised += OnSquareSelected;
                    break;

                case ActionRange.CHOOSE_MULTIPLE_SQUARES:
                    break;

                case ActionRange.CHOOSE_MULTIPLE_PIECES:
                    break;
                case ActionRange.AUTO:
                    action.Act();
                    EndAction(_currentAction);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// On a square is chosen use the chosen Action on that square and end it. 
        /// After that, start the next action.
        /// </summary>
        /// <param name="selSquare"></param>
        private void OnSquareSelected(Square selSquare)
        {
            _chosenSquare = selSquare;
            _chosenPiece = selSquare.Piece != null ? selSquare.Piece : null;
            EndAction(_currentAction);
        }

        /// <summary>
        /// End the current action
        /// </summary>
        /// <param name="startNextAction">Start next action on the queue if wanted.</param>
        public void EndCurrentAction(bool startNextAction)
        {
            if (_currentAction != null)
                EndAction(_currentAction, startNextAction);
        }

        /// <summary>
        /// End the input action.
        /// Unlisten the OnSquareSelected event.
        /// </summary>
        /// <param name="action">Action to end</param>
        /// <param name="startNextAction">Start next action on the queue if wanted.</param>
        public void EndAction(Action action, bool startNextAction = true)
        {
            Debug.Log("EndAction");
            switch (action.Range)
            {
                case ActionRange.CHOOSE_SQUARE:
                    SquareEventList.Instance.SquareSelected.OnEventRaised -= OnSquareSelected;
                    DuelManager.Instance.PhaseMachine.Change(Phase.ACTION);
                    if (_chosenSquare != null)
                    {
                        action.ActOnSquare(_chosenSquare, _currentCard.Owner.Faction);
                    }
                    break;

                case ActionRange.CHOOSE_PIECE:
                    SquareEventList.Instance.SquareSelected.OnEventRaised -= OnSquareSelected;
                    if (_chosenPiece != null)
                    {
                        action.ActOnSquare(_chosenSquare, _currentCard.Owner.Faction);
                    }
                    DuelManager.Instance.PhaseMachine.Change(Phase.ACTION);
                    break;

                case ActionRange.CHOOSE_MULTIPLE_SQUARES:
                    break;

                case ActionRange.CHOOSE_MULTIPLE_PIECES:
                    break;
                default:
                    break;
            }
            if (startNextAction)
                StartNextAction();
        }
    }
}

