using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
namespace Penwyn.Game
{
    public class EnergyTracker : MonoBehaviour
    {
        public TMP_Text EnergyTxt;
        private DeckManager _ownerDM;

        private void Awake()
        {
            _ownerDM = GetComponent<DeckManager>();
        }

        public void ConnectEvents()
        {
            _ownerDM.Owner.Data.Energy.CurrentValueChanged += EnergyChanged;
        }

        private void OnDisable()
        {
            _ownerDM.Owner.Data.Energy.CurrentValueChanged -= EnergyChanged;
        }

        private void EnergyChanged()
        {
            EnergyTxt.text = _ownerDM.Owner.Data.Energy.CurrentValue.ToString();
        }
    }
}

