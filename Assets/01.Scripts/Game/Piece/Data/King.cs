using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    [CreateAssetMenu(menuName = "Game/Piece/King")]
    public class King : PieceData
    {
        public override List<Square> FindLegalMoves(Square pieceSquare, Square[,] board, Faction faction)
        {
            List<Square> kingMoves = new List<Square>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Square square = new Square(i + pieceSquare.Rank, j + pieceSquare.File);//Checking Square
                    if (square.IsInGrid && board[square.Rank, square.File] != null)
                    {
                        //Debug.Log(Square.GetName(square.Rank, square.File));
                        kingMoves.Add(board[square.Rank, square.File]);
                    }
                }
            }
            return kingMoves;
        }
    }
}

