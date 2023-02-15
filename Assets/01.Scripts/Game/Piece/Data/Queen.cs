using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    [CreateAssetMenu(menuName = "Game/Piece/Queen")]
    public class Queen : PieceData
    {
        public override List<Square> FindLegalMoves(Square pieceSquare, Square[,] board, Faction faction)
        {
            List<Square> queenMoves = new List<Square>();
            for (int i = -8; i <= 8; i++)
            {
                for (int j = -8; j <= 8; j++)
                {
                    Square square = new Square(i + pieceSquare.Rank, j + pieceSquare.File);//Checking Square
                    if (square.IsInGrid && board[square.Rank, square.File] != null && ((i == 0 || j == 0) || (Mathf.Abs(i) == Mathf.Abs(j))))
                    {
                        queenMoves.Add(board[square.Rank, square.File]);
                    }
                }
            }
            return queenMoves;
        }
    }
}

