using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    public class Piece : MonoBehaviour
    {
        private SpriteRenderer _sprRenderer;
        private PieceData _data;
        private Faction _faction = Faction.WHITE;


        private void Awake()
        {
            _sprRenderer = GetComponent<SpriteRenderer>();
        }

        public void Load(PieceData data, Faction faction = Faction.WHITE)
        {
            this._data = data;
            _faction = faction;
            this._sprRenderer.sprite = _faction == Faction.WHITE ? _data.WhiteSprite : _data.BlackSprite;
        }

        public PieceData Data { get => _data; }
        public Faction Faction { get => _faction; }
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
