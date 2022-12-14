using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    public class Square
    {
        public static string[] RANK = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" };
        public static string[] FILE = new string[] { "a", "b", "c", "d", "e", "f", "g", "h" };

        private int rank = 0;
        private int file = 0;

        public Square() { }

        public Square(int rank, int file)
        {
            this.rank = rank;
            this.file = file;
        }

        public int Rank { get => rank; }
        public int File { get => file; }

        public override string ToString()
        {
            return $"{FILE[file]}{RANK[rank]}";
        }
    }
}

