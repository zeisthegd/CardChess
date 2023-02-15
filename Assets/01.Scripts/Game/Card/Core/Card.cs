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
        public Image Background;
        public Image CardAvatar;

        [Header("Text")]
        public TMP_Text NameTxt;
        public TMP_Text CostTxt;

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
            CostTxt?.SetText(data.Cost.GetCurrentValueText());
        }

        public virtual void ShowInfo(bool isShown = true)
        {
            NameTxt.enabled = isShown;
            CostTxt.enabled = isShown;
        }

        public void EnergyCheck()
        {
            if (CostTxt != null)
                CostTxt.color = EnoughEnergy ? Color.white : Color.red;
        }


        public virtual void SetCanvasOrder(int order)
        {
            GetComponentInChildren<Canvas>().sortingOrder = order;
        }

        private void OnEnable()
        {
            ShowInfo(false);
            ConnectEvents();
        }
        private void OnDisable()
        {
            DisconnectEvents();
        }


        protected virtual void ConnectEvents()
        {
            ProtagonistEventList.Instance.EnergyAltered.OnEventRaised += EnergyCheck;
        }

        protected virtual void DisconnectEvents()
        {
            ProtagonistEventList.Instance.EnergyAltered.OnEventRaised -= EnergyCheck;
        }

        public void OnMouseEnter()
        {
            ShowInfo(true);
            CardEventList.Instance.PointerEnter.RaiseEvent(this);
        }

        public void OnMouseExit()
        {
            ShowInfo(false);
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
