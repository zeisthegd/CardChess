using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

using NaughtyAttributes;
using Penwyn.Tools;

namespace Penwyn.Game
{
    public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
    {
        public Duelist DuelistPrefab;

        public Duelist MainPlayer;//Player of this current client/device.
        public Duelist OtherPlayer;//Player of the other client/device.

        public Faction HostFaction = Faction.WHITE;

        public DuelistData DefaultData;

        private PhotonView _photonView;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        private void OnEnable()
        {
            HostFaction = Faction.WHITE;
        }

        public void CreatePlayers(GameMode mode)
        {
            switch (mode)
            {
                case GameMode.PVP:
                    CreatePVP();
                    break;
                case GameMode.PVE:
                    CreatePVE();
                    break;
                case GameMode.AIvAI:
                    CreateAIvAI();
                    break;
            }
            ResetPlayersEnergy();
        }

        public void CreatePVP()
        {
            MainPlayer = CreateAPlayer(PhotonNetwork.IsMasterClient ? HostFaction : GetOppositeFactionOfHost());
            OtherPlayer = CreateAPlayer(PhotonNetwork.IsMasterClient ? GetOppositeFactionOfHost() : HostFaction);
        }

        public void CreatePVE()
        {
            MainPlayer = CreateAPlayer(HostFaction);
            OtherPlayer = CreateAPlayer(GetOppositeFactionOfHost());
        }

        public void CreateAIvAI()
        {

        }

        public void ChangeHostStartingFaction(Faction faction)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _photonView.RPC(nameof(RPC_ChangeHostStartingFaction), RpcTarget.All, faction);
            }
        }

        [PunRPC]
        private void RPC_ChangeHostStartingFaction(Faction faction)
        {
            HostFaction = faction;
        }

        public void ResetPlayersEnergy()
        {
            MainPlayer.Data.Energy.CurrentValue = MainPlayer.Faction == Faction.WHITE ? 3 : 1;
            OtherPlayer.Data.Energy.CurrentValue = OtherPlayer.Faction == Faction.WHITE ? 3 : 1;
        }


        public Duelist CreateAPlayer(Faction faction = Faction.WHITE)
        {
            Duelist duelist = Instantiate(DuelistPrefab);
            duelist.Load(DefaultData.Clone(), faction);
            return duelist;
        }

        public void CreateAnAIPlayer()
        {

        }

        public Faction GetOppositeFactionOfHost()
        {
            return HostFaction == Faction.WHITE ? Faction.BLACK : Faction.WHITE;
        }
    }
}

