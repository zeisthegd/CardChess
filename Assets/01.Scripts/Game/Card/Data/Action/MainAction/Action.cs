using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Tools;

using NaughtyAttributes;

namespace Penwyn.Game
{
    public abstract class Action : ScriptableObject
    {
        public ActionRange Range;
        public IntValue ActionValue;
        [TextArea] public string Description;

        [ReadOnly][SerializeField] protected bool _requiredChosenSquareSameColor = false;
        [ReadOnly][SerializeField] protected bool _requiredChosenPieceSameColor = false;


        public virtual void StartAction() { }
        public virtual void StartAction(Faction faction) { }
        public virtual void EndAction() { }
        public virtual void Act() { }
        public virtual void ActOnSquare(Square square, Faction faction) { }
        public virtual void ActOnPiece(Piece piece, Faction faction) { }
        public virtual void ActOnManySquares(List<Square> squares, Faction faction) { }
        public virtual void ActOnManyPieces(List<Piece> pieces, Faction faction) { }

        public abstract string GetDescription();
        public abstract DisplayInfo GetInfo();

        void OnEnable()
        {
            ActionValue.Reset();
        }

        void OnDisable()
        {
            ActionValue.Reset();
        }
        public bool RequiredChosenSquareSameColor { get => _requiredChosenSquareSameColor; }
        public bool RequiredChosenPieceSameColor { get => _requiredChosenPieceSameColor; }
    }

    public enum ActionRange
    {
        CHOOSE_SQUARE,
        CHOOSE_MULTIPLE_SQUARES,
        CHOOSE_PIECE,
        CHOOSE_MULTIPLE_PIECES,
        AUTO
    }


}




