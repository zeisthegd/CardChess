using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Tools;

namespace Penwyn.Game
{
    [CreateAssetMenu(menuName ="Game/Card/Action/Create Piece")]
    public class CreatePieceAction : Action
    {
        public PieceIndex PieceToCreate;

        public override void ActOnSquare(Square square)
        {
            base.ActOnSquare(square);
            DuelManager.Instance.BoardView.RPC_CreatePiece(PieceToCreate, square.Rank, square.File);
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




