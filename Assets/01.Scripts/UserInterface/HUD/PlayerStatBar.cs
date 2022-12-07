using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Penwyn.UI
{
    public class PlayerStatBar : MonoBehaviour
    {
        [SerializeField] Image totalLife;
        [SerializeField] Image totalEnergy;

        [SerializeField] Image currentLife;
        [SerializeField] Image currentEnergy;

        [SerializeField] List<float> fillThreshold;
    }
}