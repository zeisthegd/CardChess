using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    [System.Serializable]
    public class StateMachine<T>
    {
        public T CurrentState { get; protected set; }
        public T PreviousState { get; protected set; }

        public StateMachine(T initState)
        {
            CurrentState = initState;
        }

        public virtual void Change(T newState)
        {
            PreviousState = CurrentState;
            CurrentState = newState;
        }

        public virtual bool Is(T newState)
        {
            return CurrentState.Equals(newState);
        }
    }
}
