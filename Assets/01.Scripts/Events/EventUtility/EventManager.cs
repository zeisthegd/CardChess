using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    /// <summary>
    /// Make sure the event lists got their OnEnable() Called.
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        void Awake()
        {
            CardEventList.Instance = new CardEventList();
            GameEventList.Instance = new GameEventList();
            ProtagonistEventList.Instance = new ProtagonistEventList();
            SquareEventList.Instance = new SquareEventList();
        }
    }
}
