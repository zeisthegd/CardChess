using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    [CreateAssetMenu(menuName = "Game/Piece/Knight")]
    public class Knight : PieceData
    {
        public override List<Square> FindLegalMoves(Square pieceSquare, Square[,] board, Faction faction)
        {
            List<Square> knightMoves = new List<Square>();
            knightMoves.Add(pieceSquare);
            for (int i = -2; i <= 2; i++)
            {
                for (int j = -2; j <= 2; j++)
                {
                    Square square = new Square(i + pieceSquare.Rank, j + pieceSquare.File);//Checking Square
                    if (square.IsInGrid && board[square.Rank, square.File] != null && i != 0 && j != 0 && (Mathf.Abs(i) != Mathf.Abs(j)))
                    {
                        Debug.Log(Square.GetName(square.Rank, square.File));
                        knightMoves.Add(board[square.Rank, square.File]);
                    }
                }
            }
            return knightMoves;
        }
    }
}

