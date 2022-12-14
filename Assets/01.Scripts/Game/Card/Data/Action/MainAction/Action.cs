using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Tools;

namespace Penwyn.Game
{
    public abstract class Action : ScriptableObject
    {
        public ActionRange Range;
        public IntValue ActionValue;
        [TextArea] public string Description;
        public virtual void Act() { }

        public abstract string GetDescription();
        public abstract DisplayInfo GetInfo();

        void OnEnable()
        {
            ActionValue.Reset();
        }

        void OnDisable()
        {
            ActionValue.Reset();
        }
    }

    public enum ActionRange
    {
        CHOSEN_UNIT,
        ALL_OPPONENTS,
        ALL_ALLIES
    }
}




