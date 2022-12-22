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
            clone.Actions.Clear();
            for (int i = 0; i < this.Actions.Count; i++)
            {
                clone.Actions.Add(Instantiate(this.Actions[i]));
            }
            return clone;
        }
        
        public string GetDescription()
        {
            string des = "";

            return des;
        }
    }
}
