using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon;
using Photon.Pun;

using NaughtyAttributes;

using Penwyn.Tools;

namespace Penwyn.Game
{
    public class BoardView : MonoBehaviourPun
    {
        public Piece PiecePrefab;
        public SquareView SquareViewPrefab;
        public BoardViewMode ViewMode;

        public Pawn Pawn;
        public Knight Knight;
        public Bishop Bishop;
        public Rook Rook;
        public Queen Queen;
        public King King;

        private Square[,] _board;
        private SquareView[,] _boardView;

        private void OnEnable()
        {
            InitBoardCoordinate();
        }

        public void InitBoardCoordinate()
        {
            _board = new Square[8, 8];
            _boardView = new SquareView[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    _board[i, j] = new Square(i, j);
                }
            }
        }

        public void SpawnBoardSquares()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    SquareView newSqV = Instantiate(SquareViewPrefab, this.transform.position, Quaternion.identity, this.transform);
                    newSqV.SetData(_board[i, j]);
                    newSqV.transform.position = ArrayPosToWorldPos(i, j);
                    _boardView[i, j] = newSqV;
                }
            }
        }

        public void SpawnKings()
        {
            CreatePiece(PieceIndex.K, 4, 0, Faction.WHITE);
            CreatePiece(PieceIndex.K, 4, 7, Faction.BLACK);
        }

        public void CreatePiece(PieceIndex index, int file, int rank, Faction userFaction)
        {
            if (_board[file, rank].Piece == null)
            {
                photonView.RPC(nameof(RPC_CreatePiece), RpcTarget.All, new object[] { index, file, rank, userFaction });

            }
            else
            {
                Announcer.Instance.Announce("Square Is Occupied By A Piece.");
            }
        }

        [PunRPC]
        public void RPC_CreatePiece(PieceIndex index, int file, int rank, Faction faction)
        {
            Piece newPiece = Instantiate(PiecePrefab);
            switch (index)
            {
                case PieceIndex.P:
                    newPiece.Load(Pawn, faction);
                    break;
                case PieceIndex.N:
                    newPiece.Load(Knight, faction);
                    break;
                case PieceIndex.B:
                    newPiece.Load(Bishop, faction);
                    break;
                case PieceIndex.R:
                    newPiece.Load(Rook, faction);
                    break;
                case PieceIndex.Q:
                    newPiece.Load(Queen, faction);
                    break;
                case PieceIndex.K:
                    newPiece.Load(King, faction);
                    break;
                default:
                    break;
            }
            newPiece.transform.position = ArrayPosToWorldPos(_board[file, rank]);
            _board[file, rank].Piece = newPiece;
            OccupySquares(newPiece, _board[file, rank]);
            Debug.Log("Create: " + Square.GetName(rank, file));
        }

        private void OccupySquares(Piece piece, Square pieceSquare)
        {
            List<Square> legalMoves = piece.Data.FindLegalMoves(pieceSquare, this._board, piece.Faction);
            foreach (Square square in legalMoves)
            {
                square.Faction = piece.Faction;
                _boardView[square.File, square.Rank].SetColor();
            }
        }

        private Vector3 ArrayPosToWorldPos(Square square)
        {
            return ArrayPosToWorldPos(square.File, square.Rank);
        }

        private Vector3 ArrayPosToWorldPos(int file, int rank)
        {
            if (ViewMode == BoardViewMode.WHITE)
            {
                return this.transform.position + new Vector3(file - 4, rank - 4);
            }

            if (ViewMode == BoardViewMode.BLACK)
            {
                return this.transform.position + new Vector3(4 - file, 4 - rank);
            }
            return Vector3.zero;
        }
    }

    public enum BoardViewMode
    {
        WHITE,
        BLACK
    }
}

