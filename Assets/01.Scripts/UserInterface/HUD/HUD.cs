using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;
using TMPro;

using Penwyn.Game;
using Penwyn.Tools;

namespace Penwyn.UI
{
    public class HUD : SingletonMonoBehaviour<HUD>
    {
        public TMP_Text CurrentTurnCount;
        public TMP_Text CurrentTurnFaction;

        public virtual void DuelStart()
        {
            DuelManager.Instance.CurrentTurnCount.CurrentValueChanged += UpdateCurrentTurnCount;
            GameEventList.Instance.TurnChanged.OnEventRaised += UpdateCurrentTurnFaction;
            GameEventList.Instance.MatchEnded.OnEventRaised += DuelEnd;

            UpdateCurrentTurnCount();
            UpdateCurrentTurnFaction();
        }

        public virtual void DuelEnd()
        {
            DuelManager.Instance.CurrentTurnCount.CurrentValueChanged -= UpdateCurrentTurnCount;
            GameEventList.Instance.TurnChanged.OnEventRaised -= UpdateCurrentTurnFaction;
            GameEventList.Instance.MatchEnded.OnEventRaised -= DuelEnd;
        }

        private void UpdateCurrentTurnCount()
        {
            if (CurrentTurnCount != null)
                CurrentTurnCount.SetText($"Turn: {DuelManager.Instance.CurrentTurnCount.CurrentValue}/{DuelManager.Instance.DuelSettings.Turn}");
            else
                Debug.LogWarning("No CurrentTurnCountTxt Found!");
        }

        private void UpdateCurrentTurnFaction()
        {
            string factionTxt = DuelManager.Instance.CurrentFactionTurn == Faction.WHITE ? $"<color=white>{DuelManager.Instance.CurrentFactionTurn.ToString()}</color>" : $"<color=black>{DuelManager.Instance.CurrentFactionTurn.ToString()}</color>";
            CurrentTurnFaction.SetText(factionTxt + "\nTo Move");
        }

        private void OnEnable()
        {
            GameEventList.Instance.MatchStarted.OnEventRaised += DuelStart;

        }

        private void OnDisable()
        {
            GameEventList.Instance.MatchStarted.OnEventRaised -= DuelStart;
        }
    }
}

