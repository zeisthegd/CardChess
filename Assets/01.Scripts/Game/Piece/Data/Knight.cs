using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    [CreateAssetMenu(menuName = "Game/Piece/Knight")]
    public class Knight : PieceData
    {
        public override List<Square> FindLegalMoves()
        {
            throw new System.NotImplementedException();
        }
    }
}

