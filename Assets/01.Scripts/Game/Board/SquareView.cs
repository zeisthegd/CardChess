using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    public class SquareView : MonoBehaviour
    {
        public Sprite EmptySprite;
        public Sprite WhiteSprite;
        public Sprite BlackSprite;

        private Square _square;
        private SpriteRenderer _spriteRender;

        private void Awake()
        {
            _spriteRender = GetComponent<SpriteRenderer>();
        }

        public void SetData(Square square)
        {
            this._square = square;
            ClearSquare();
        }

        public void SetColor()
        {
            if (_square.File % 2 != 0 && _square.Rank % 2 != 0 || _square.File % 2 == 0 && _square.Rank % 2 == 0)//Black if rank and file are both odd or even.
            {
                _spriteRender.sprite = BlackSprite;
            }
            else
                _spriteRender.sprite = WhiteSprite;
        }

        public void SetBlack()
        {
            _spriteRender.sprite = BlackSprite;
        }

        public void SetWhite()
        {
            _spriteRender.sprite = WhiteSprite;
        }

        public void ClearSquare()
        {
            _spriteRender.sprite = EmptySprite;
        }

        public void OnMouseEnter()
        {
            SquareEventList.Instance.SquareHovered.RaiseEvent(this._square);
        }

        public void OnMouseSelect()
        {
            SquareEventList.Instance.SquareSelected.RaiseEvent(this._square);
        }
    }

}
