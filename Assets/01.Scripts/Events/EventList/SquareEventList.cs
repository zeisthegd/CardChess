using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    public class SquareEventList
    {
        public SquareEventChannel SquareSelected;
        public SquareEventChannel SquareHovered;

        public static SquareEventList Instance;

        public SquareEventList()
        {
            SquareSelected = new SquareEventChannel();
            SquareHovered = new SquareEventChannel();
        }
    }

}
