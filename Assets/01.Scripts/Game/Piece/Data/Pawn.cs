using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    [CreateAssetMenu(menuName = "Game/Piece/Pawn")]
    public class Pawn : PieceData
    {
        public override List<Square> FindLegalMoves(Square pieceSquare, Square[,] board, Faction faction)
        {
            List<Square> pawnMoves = new List<Square>();
            for (int i = -1; i <= 1; i++)
            {
                Square square = null;
                if (faction == Faction.WHITE)
                    square = new Square(pieceSquare.Rank + 1, i + pieceSquare.File);//Move forward for white pawn.
                else if (faction == Faction.BLACK)
                    square = new Square(pieceSquare.Rank - 1, i + pieceSquare.File);//Move backward for black pawn.

                if (square.IsInGrid && board[square.Rank, square.File] != null)
                {
                    //Debug.Log(Square.GetName(square.Rank, square.File));
                    pawnMoves.Add(board[square.Rank, square.File]);
                }
            }
            return pawnMoves;
        }
    }
}

