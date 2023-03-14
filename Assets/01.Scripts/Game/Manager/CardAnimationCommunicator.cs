using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;


namespace Penwyn.Game
{
    public class CardAnimationCommunicator : MonoBehaviourPun
    {
        private DeckManager _ownerDM;

        public DeckManager OwnerDM { get => _ownerDM; }

        private void Awake()
        {
            FindOwnerDeckManager();
        }
        public void Hover(Card card)
        {
            if (photonView.IsMine)
            {
                photonView.RPC(nameof(RPC_Hover), RpcTarget.Others, _ownerDM.HandPile.GetCardIndex(card));
            }
        }

        [PunRPC]
        private void RPC_Hover(int cardIndex)
        {
            Debug.Log($"RPC_Hover: " + cardIndex);
            _ownerDM.CardHandAnimationController.Hover(_ownerDM.HandPile.GetCard(cardIndex));
        }

        public void ExitHover(Card card)
        {
            if (photonView.IsMine)
            {
                photonView.RPC(nameof(RPC_ExitHover), RpcTarget.Others, _ownerDM.HandPile.GetCardIndex(card));
            }
        }

        [PunRPC]
        private void RPC_ExitHover(int cardIndex)
        {
            Debug.Log($"RPC_ExitHover: " + cardIndex);
            _ownerDM.CardHandAnimationController.Exit(_ownerDM.HandPile.GetCard(cardIndex));
        }

        private void FindOwnerDeckManager()
        {
            if (photonView.IsMine)
                _ownerDM = DuelManager.Instance.MasterDM;
            else _ownerDM = DuelManager.Instance.GuestDM;
        }

    }

}

