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
        private Faction _faction = Faction.NONE;
        private Piece _piece;

        public Square() { }

        public Square(int _file, int _rank)
        {
            this._file = _file;//i
            this._rank = _rank;//j
        }

        public int Rank { get => _rank; }
        public int File { get => _file; }
        public Piece Piece { get => _piece; set => _piece = value; }
        public Faction Faction { get => _faction; set => _faction = value; }
        public bool IsInGrid => Rank > 0 && File >= 0 && Rank < 8 && File < 8;

        public override string ToString()
        {
            return GetName(_rank, _file);
        }

        public static string GetName(int rank, int file)
        {
            if (rank >= 0 && file >= 0 && rank < 8 && file < 8)
                return $"{FILE[file]}{RANK[rank]}";
            return "SQR_NotInGrid";
        }
    }
}

