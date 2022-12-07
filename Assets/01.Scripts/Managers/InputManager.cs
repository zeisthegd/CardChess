using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Tools;

namespace Penwyn.Game
{
    [CreateAssetMenu(menuName = "Managers/Input Manager")]
    public class InputManager : SingletonScriptableObject<InputManager>
    {
        public InputReader InputReader;
        public bool ShouldHideCursor = false;

        protected virtual void Start()
        {
            Initialization();
        }

        public virtual void Initialization()
        {
            HideCursor();
        }

        public virtual void HideCursor()
        {
            Cursor.visible = !ShouldHideCursor;
        }
    }
}
