using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using TMPro;
namespace Penwyn.Game
{
    public class Card : MonoBehaviour, IPointerClickHandler
    {
        [Header("Data")]
        public CardData Data;

        [Header("UI")]
        public Image Frame;
        public Image BackViewFrame;
        public Image CardAvatar;
        public Sprite WhiteFrameSprite;
        public Sprite BlackFrameSprite;

        [Header("Text")]
        public TMP_Text NameTxt;

        [Header("Info")]
        public RectTransform InfoPanel;
        public EventTrigger EventTrigger;
        private Duelist _owner;


        public virtual void Use(List<Piece> targetList)
        {

        }

        public virtual void SetData(CardData data)
        {
            this.Data = data;
            DisplayCard(data);
        }

        public virtual void DisplayCard(CardData data)
        {
            CardAvatar.sprite = data.Avatar;
            NameTxt?.SetText(data.Name);
            gameObject.name = data.Name + "_UI";
        }

        public virtual void ChangeColor()
        {
            if (Frame == null || WhiteFrameSprite == null || BlackFrameSprite == null)
                return;
            switch (_owner.Faction)
            {
                case Faction.WHITE:
                    Frame.sprite = WhiteFrameSprite;
                    break;
                case Faction.BLACK:
                    Frame.sprite = BlackFrameSprite;
                    break;
            }
        }

        public virtual void ShowInfo()
        {
        }


        public virtual void SetCanvasOrder(int order)
        {
            GetComponentInChildren<Canvas>().sortingOrder = order;
        }

        private void OnEnable()
        {
            ConnectEvents();
        }
        private void OnDisable()
        {
            DisconnectEvents();
        }


        protected virtual void ConnectEvents()
        {
        }

        protected virtual void DisconnectEvents()
        {

        }

        public void OnMouseEnter()
        {
            ShowInfo();
            CardEventList.Instance.PointerEnter.RaiseEvent(this);
        }

        public void OnMouseExit()
        {
            ShowInfo();
            CardEventList.Instance.PointerExit.RaiseEvent(this);
        }

        public void OnPointerClick(PointerEventData data)
        {
            if (data.button == PointerEventData.InputButton.Left)
                CardEventList.Instance.PointerSelect.RaiseEvent(this);
        }

        public void OnMouseDragging()
        {
            CardEventList.Instance.PointerDragging.RaiseEvent(this);
        }

        public void OnMouseBeginDrag()
        {
            CardEventList.Instance.PointerBeginDrag.RaiseEvent(this);
        }

        public bool EnoughEnergy { get => Data.Cost.CurrentValue <= Owner.Data.Energy.CurrentValue; }
        public Duelist Owner { get => _owner; set => _owner = value; }
    }
}
