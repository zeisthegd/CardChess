using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;


namespace Penwyn.Game
{
    public class CardAnimationCommunicator : MonoBehaviourPun
    {
        private DeckManager _ownerDM;


        private void Awake()
        {
            FindOwnerDeckManager();
        }

        /// <summary>
        /// If this object (photonView.IsMine) RPC the Hover function on other clients.
        /// </summary>
        /// <param name="card"></param>
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
            _ownerDM.CardHandAnimationController.Hover(_ownerDM.HandPile.GetCard(cardIndex));
        }

        /// <summary>
        /// If this object (photonView.IsMine) RPC the ExitHover function on other clients.
        /// </summary>
        /// <param name="card"></param>
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
            _ownerDM.CardHandAnimationController.Exit(_ownerDM.HandPile.GetCard(cardIndex));
        }

        /// <summary>
        /// If this object (photonView.IsMine) RPC the ExitHover function on other clients.
        /// </summary>
        /// <param name="card"></param>
        public void Discard(Card card)
        {
            if (photonView.IsMine)
            {
                photonView.RPC(nameof(RPC_Discard), RpcTarget.Others, _ownerDM.HandPile.GetCardIndex(card));
            }
        }

        [PunRPC]
        private void RPC_Discard(int cardIndex)
        {
            Debug.Log(_ownerDM.name + $": {cardIndex}");
            _ownerDM.Discard(_ownerDM.HandPile.GetCard(cardIndex));
        }

        private void FindOwnerDeckManager()
        {
            if (photonView.IsMine)
                _ownerDM = DuelManager.Instance.MasterDM;
            else _ownerDM = DuelManager.Instance.GuestDM;
        }
        public DeckManager OwnerDM { get => _ownerDM; }

    }

}

