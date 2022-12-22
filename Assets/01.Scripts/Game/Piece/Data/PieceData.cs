using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    public abstract class PieceData : ScriptableObject
    {
        public Sprite Sprite;
        public abstract List<Square> FindLegalMoves();
    }
}