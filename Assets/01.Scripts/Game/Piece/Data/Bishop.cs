using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    [CreateAssetMenu(menuName = "Game/Piece/Bishop")]
    public class Bishop : PieceData
    {
        public override List<Square> FindLegalMoves(Square pieceSquare, Square[,] board, Faction faction)
        {
            List<Square> bishopMoves = new List<Square>();
            bishopMoves.Add(pieceSquare);
            for (int i = -8; i <= 8; i++)
            {
                for (int j = -8; j <= 8; j++)
                {
                    Square square = new Square(i + pieceSquare.Rank, j + pieceSquare.File);//Checking Square
                    if (square.IsInGrid && board[square.Rank, square.File] != null && (Mathf.Abs(i) == Mathf.Abs(j)))
                    {
                        // Debug.Log(Square.GetName(square.Rank, square.File));
                        bishopMoves.Add(board[square.Rank, square.File]);
                    }
                }
            }
            return bishopMoves;
        }
    }
}

