using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Tools;

namespace Penwyn.Game
{
    [CreateAssetMenu(menuName = "Game/Card/Action/Create Piece")]
    public class CreatePieceAction : Action
    {
        public PieceIndex PieceToCreate;

        private void OnEnable()
        {
            _requiredChosenSquareSameColor = true;
            _requiredChosenPieceSameColor = false;
        }

        public override void ActOnSquare(Square square, Faction faction)
        {
            base.ActOnSquare(square, faction);
            DuelManager.Instance.BoardView.CreatePiece(PieceToCreate, square.File, square.Rank, faction);
        }

        public override string GetDescription()
        {
            return "";
        }

        public override DisplayInfo GetInfo()
        {
            return new DisplayInfo();
        }
    }

}




