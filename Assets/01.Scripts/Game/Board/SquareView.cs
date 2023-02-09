using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Tools;

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
            _square.Faction = Faction.NONE;
            SetColor();
            gameObject.name = $"Square[{Square.GetName(this._square.Rank, this._square.File)}]";
        }

        public void SetColor()
        {
            switch (_square.Faction)
            {
                case Faction.WHITE:
                    SetWhite();
                    break;
                case Faction.BLACK:
                    SetBlack();
                    break;
                case Faction.NONE:
                    ClearSquare();
                    break;
                default:
                    break;
            }
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
            Debug.Log("On Mouse Select");
            SquareEventList.Instance.SquareSelected.RaiseEvent(this._square);
        }
    }

}
