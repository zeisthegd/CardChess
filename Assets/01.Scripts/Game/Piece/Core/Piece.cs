using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    public class Piece : MonoBehaviour
    {
        private PieceData _data;
        
        public void Load(PieceData data)
        {
            this._data = data;
        }
    }

    public enum PieceIndex
    {
        P,
        N,
        B,
        R,
        Q,
        K
    }

}
