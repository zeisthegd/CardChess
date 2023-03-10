﻿using System.Linq;
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
    [CreateAssetMenu(menuName = "Managers/Player Manager")]
    public class PlayerManager : SingletonScriptableObject<PlayerManager>
    {
        public Duelist DuelistPrefab;

        public Duelist MainPlayer;//Player of this current client/device.
        public Duelist OtherPlayer;//Player of the other client/device.

        public Faction MasterClientDefaultFaction = Faction.WHITE;
        public Faction OtherClientDefaultFaction = Faction.BLACK;

        public DuelistData DefaultData;

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
        }

        public void CreatePVP()
        {
            MainPlayer = CreateAPlayer(PhotonNetwork.IsMasterClient ? MasterClientDefaultFaction : OtherClientDefaultFaction);
            OtherPlayer = CreateAPlayer(PhotonNetwork.IsMasterClient ? OtherClientDefaultFaction : MasterClientDefaultFaction);
        }

        public void CreatePVE()
        {
            MainPlayer = CreateAPlayer(MasterClientDefaultFaction);
            OtherPlayer = CreateAPlayer(OtherClientDefaultFaction);
        }

        public void CreateAIvAI()
        {

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
    }
}

