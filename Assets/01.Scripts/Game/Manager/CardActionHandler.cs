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

        public void GenerateActionQueue(Card card)
        {
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
                Debug.Log("No more action.");
        }

        public void StartAction(Action action)
        {
            switch (action.Range)
            {
                case ActionRange.CHOOSE_SQUARE:
                    DuelManager.Instance.PhaseMachine.Change(Phase.CHOOSE_SQUARE);
                    Announcer.Instance.Announce("Please choose a square");
                    Debug.Log("Changed to choose square.");
                    SquareEventList.Instance.SquareSelected.OnEventRaised += OnSquareSelected;
                    break;

                case ActionRange.CHOOSE_PIECE:
                    Announcer.Instance.Announce("Please choose a piece");
                    Debug.Log("Changed to choose piece.");
                    SquareEventList.Instance.SquareSelected.OnEventRaised += OnSquareSelected;
                    break;

                case ActionRange.CHOOSE_MULTIPLE_SQUARES:
                    break;

                case ActionRange.CHOOSE_MULTIPLE_PIECES:
                    break;

                default:
                    break;
            }
        }

        private void OnSquareSelected(Square selSquare)
        {
            _chosenSquare = selSquare;
            _chosenPiece = selSquare.Piece != null ? selSquare.Piece : null;
            EndAction(_currentAction);
        }

        public void EndAction(Action action)
        {
            switch (action.Range)
            {
                case ActionRange.CHOOSE_SQUARE:
                    DuelManager.Instance.PhaseMachine.Change(Phase.ACTION);
                    if (_chosenSquare != null)
                    {
                        action.ActOnSquare(_chosenSquare);
                        Debug.Log($"EndSelected: {_chosenSquare.ToString()}");
                    }
                    SquareEventList.Instance.SquareSelected.OnEventRaised -= OnSquareSelected;
                    break;

                case ActionRange.CHOOSE_PIECE:
                    DuelManager.Instance.PhaseMachine.Change(Phase.ACTION);
                    if (_chosenSquare.Piece != null)
                        Debug.Log($"EndSelected: {_chosenSquare.ToString()}");
                    SquareEventList.Instance.SquareSelected.OnEventRaised -= OnSquareSelected;
                    break;

                case ActionRange.CHOOSE_MULTIPLE_SQUARES:
                    break;

                case ActionRange.CHOOSE_MULTIPLE_PIECES:
                    break;

                default:
                    break;
            }
            StartNextAction();
        }
    }
}

