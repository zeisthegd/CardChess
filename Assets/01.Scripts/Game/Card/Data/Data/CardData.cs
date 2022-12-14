using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Tools;

namespace Penwyn.Game
{
    [CreateAssetMenu(menuName ="Game/CardData",fileName ="Card")]
    public class CardData : ScriptableObject
    {
        [Header("Graphics")]
        public Sprite Avatar;

        [Header("Data")]
        public string Name;
        public IntValue Cost;
        public List<Action> Actions;


        [Header("Scriptable Data")]
        public Category Category;



        public virtual CardData Clone()
        {
            CardData clone = Instantiate(this);
            for (int i = 0; i < this.Actions.Count; i++)
            {
                clone.Actions.Add(Instantiate(this.Actions[i]));
            }
            return clone;
        }

        public bool NeedTarget()
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                if(Actions[i].Range == ActionRange.CHOSEN_UNIT)
                    return true;
            }
            return false;
        }

        
        public string GetDescription()
        {
            string des = "";

            return des;
        }
    }
}
