using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    public class Piece : MonoBehaviour
    {
        private SpriteRenderer _sprRenderer;
        private PieceData _data;
        private Faction _currentFaction = Faction.WHITE;

        private void Awake()
        {
            _sprRenderer = GetComponent<SpriteRenderer>();
        }

        public void Load(PieceData data, Faction faction = Faction.WHITE)
        {
            this._data = data;
            _currentFaction = faction;
            this._sprRenderer.sprite = _currentFaction == Faction.WHITE ? _data.WhiteSprite : _data.BlackSprite;
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
