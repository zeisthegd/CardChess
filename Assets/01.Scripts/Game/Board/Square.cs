using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    public class Square
    {
        public static string[] RANK = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" };
        public static string[] FILE = new string[] { "a", "b", "c", "d", "e", "f", "g", "h" };

        private int _rank = 0;
        private int _file = 0;
        private Piece _piece;

        public Square() { }

        public Square(int _rank, int _file)
        {
            this._rank = _rank;
            this._file = _file;
        }

        public int Rank { get => _rank; }
        public int File { get => _file; }
        public Piece Piece { get => _piece; set => _piece = value; }

        public override string ToString()
        {
            return $"{FILE[_file]}{RANK[_rank]}";
        }
    }
}

