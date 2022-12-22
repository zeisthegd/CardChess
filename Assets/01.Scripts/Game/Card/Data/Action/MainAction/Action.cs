using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Tools;

namespace Penwyn.Game
{
    public abstract class Action : ScriptableObject
    {
        public ActionRange Range;
        public IntValue ActionValue;
        [TextArea] public string Description;
        
        public virtual void Act() { }
        public virtual void ActOnSquare(Square square) { }
        public virtual void ActOnPiece(Piece piece) { }
        public virtual void ActOnManySquares(List<Square> squares) { }
        public virtual void ActOnManyPieces(List<Piece> pieces) { }

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
    }

    public enum ActionRange
    {
        CHOOSE_SQUARE,
        CHOOSE_MULTIPLE_SQUARES,
        CHOOSE_PIECE,
        CHOOSE_MULTIPLE_PIECES
    }
}




