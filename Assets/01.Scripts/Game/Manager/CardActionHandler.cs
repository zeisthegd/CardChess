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
            if (SquareMetActionRequirements(selSquare))
            {
                _chosenSquare = selSquare;
                _chosenPiece = selSquare.Piece != null ? selSquare.Piece : null;
                EndAction(_currentAction);
            }
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

        /// <summary>
        /// Check if the square met the requirements of the action. Ex. Same color as the faction...
        /// </summary>
        /// <param name="square"></param>
        /// <returns></returns>
        private bool SquareMetActionRequirements(Square square)
        {
            if (_currentAction.RequiredChosenSquareSameColor && square == null)
            {
                Announcer.Instance.Announce("Please choose a square");
                return false;
            }
            if (_currentAction.RequiredChosenPieceSameColor && square.Piece == null)
            {
                Announcer.Instance.Announce("Please choose a piece");
                return false;
            }
            if (SquareChosenIsOwnerFaction(square) == false)
            {
                Debug.Log(_currentAction.RequiredChosenSquareSameColor);
                Debug.Log(square.Faction);
                Debug.Log(_currentCard.Owner.Faction);
                Announcer.Instance.Announce($"Please choose a {_currentCard.Owner.Faction} square");
                return false;
            }
            if (PieceChosenIsOwnerFaction(square.Piece) == false)
            {
                Announcer.Instance.Announce($"Please choose a {_currentCard.Owner.Faction} piece");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns true if chosen square of same color is not required. Or required and square is of the same color as the user of the action.
        /// </summary>
        /// <param name="square"></param>
        /// <returns></returns>
        private bool SquareChosenIsOwnerFaction(Square square)
        {
            if (_currentAction.RequiredChosenSquareSameColor && square == null)
                return false;
            return _currentAction.RequiredChosenSquareSameColor == false || (_currentAction.RequiredChosenSquareSameColor && (square.Faction == _currentCard.Owner.Faction));
        }

        /// <summary>
        /// Returns true if chosen piece of same color is not required. Or required and piece is of the same color as the user of the action.
        /// </summary>
        /// <param name="square"></param>
        /// <returns></returns>
        private bool PieceChosenIsOwnerFaction(Piece piece)
        {
            if (_currentAction.RequiredChosenPieceSameColor && piece == null)
                return false;
            return _currentAction.RequiredChosenPieceSameColor == false || (_currentAction.RequiredChosenPieceSameColor && (piece.Faction == _currentCard.Owner.Faction));
        }
    }
}

