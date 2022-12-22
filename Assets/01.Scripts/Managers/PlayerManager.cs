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

        public Duelist MainPlayer;
        public Duelist OtherPlayer;

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
            MainPlayer = CreateAPlayer();
            OtherPlayer = CreateAPlayer();
        }

        public void CreatePVE()
        {

        }

        public void CreateAIvAI()
        {

        }

        public Duelist CreateAPlayer()
        {
            Duelist duelist = Instantiate(DuelistPrefab);
            duelist.Data = DefaultData.Clone();
            return duelist;
        }

        public void CreateAnAIPlayer()
        {

        }
    }
}
