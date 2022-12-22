using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon;
using Photon.Pun;

using NaughtyAttributes;

namespace Penwyn.Game
{
    public class BoardView : MonoBehaviourPun
    {
        public Piece PiecePrefab;
        public SquareView SquareViewPrefab;

        public Pawn Pawn;
        public Knight Knight;
        public Bishop Bishop;
        public Rook Rook;
        public Queen Queen;
        public King King;

        private Square[,] _board;

        private void OnEnable()
        {
            InitBoardCoordinate();
        }

        public void InitBoardCoordinate()
        {
            _board = new Square[8, 8];
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
                    newSqV.transform.position = this.transform.position + new Vector3(i - 4, j - 4);
                }
            }
        }

        public void RPC_CreatePiece(PieceIndex index, int rank, int file)
        {
            photonView.RPC(nameof(CreatePiece), RpcTarget.All, new object[] { index, rank, file });
        }

        [PunRPC]
        public void CreatePiece(PieceIndex index, int rank, int file)
        {
            Piece newPiece = Instantiate(PiecePrefab);
            switch (index)
            {
                case PieceIndex.P:
                    newPiece.Load(Pawn);
                    break;
                case PieceIndex.N:
                    newPiece.Load(Knight);
                    break;
                case PieceIndex.B:
                    newPiece.Load(Bishop);
                    break;
                case PieceIndex.R:
                    newPiece.Load(Rook);
                    break;
                case PieceIndex.Q:
                    newPiece.Load(Queen);
                    break;
                case PieceIndex.K:
                    newPiece.Load(King);
                    break;
                default:
                    break;
            }
            newPiece.transform.position = this.transform.position + new Vector3(rank - 4, file - 4);
            _board[rank, file].Piece = newPiece;
        }
    }
}

