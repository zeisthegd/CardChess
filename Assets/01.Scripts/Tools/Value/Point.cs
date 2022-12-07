using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Tools
{
    [System.Serializable]
    //For data operation.
    public class Point : IEquatable<Point>
    {
        int x;
        int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(Point other)
        {
            return this.x == other.x && this.y == other.y;
        }

        public override string ToString()
        {
            return $"({x}; {y})";
        }

        public int X { get => x; }
        public int Y { get => y; }
    }
}
