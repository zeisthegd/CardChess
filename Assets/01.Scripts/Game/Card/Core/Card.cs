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

        [Header("Graphics")]
        public CardTheme Theme;

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

        public virtual void ShowNormal()
        {
            if (Frame == null || Theme.WhiteFront == null || Theme.BlackFront == null)
            {
                Debug.Log("Null Pointer Exception: CardSprite");
                return;
            }
            Frame.sprite = Theme.GetFrontSprite(_owner.Faction);

        }

        public void ShowHighlight()
        {
            Frame.sprite = Theme.GetHighlightSprite(_owner.Faction);

        }

        public void ShowBack()
        {
            if (Frame == null || Theme.WhiteFront == null || Theme.BlackBack == null)
            {
                Debug.Log("Null Pointer Exception: CardSprite");
                return;
            }
            SetInfoVisibility(false);
            Frame.sprite = Theme.GetBackSprite(_owner.Faction);

        }

        public virtual void SetInfoVisibility(bool show)
        {
            CardAvatar.gameObject.SetActive(show);
            NameTxt.gameObject.SetActive(show);
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
            CardEventList.Instance.PointerEnter.RaiseEvent(this);
        }

        public void OnMouseExit()
        {
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
