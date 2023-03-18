using System.Linq;
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
        private SquareView[,] _squareViewArray;
        private PieceDeployingCalculator _pieceDeplCal;
        private SquareHighlighter _squareHighlighter;


        private void OnEnable()
        {
            GetComponents();
            InitBoardCoordinate();
        }

        public void InitBoardCoordinate()
        {
            _board = new Square[8, 8];
            _squareViewArray = new SquareView[8, 8];
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
                    _squareViewArray[i, j] = newSqV;
                }
            }
        }

        public void SpawnKings()
        {
            if (photonView.IsMine)
            {
                DeployPiece(PieceIndex.K, 4, 0, Faction.WHITE);
                DeployPiece(PieceIndex.K, 4, 7, Faction.BLACK);
            }
        }

        public void DeployPiece(PieceIndex index, int file, int rank, Faction userFaction)
        {
            if (_board[file, rank].Piece == null)
            {
                photonView.RPC(nameof(RPC_DeployPiece), RpcTarget.All, new object[] { index, file, rank, userFaction });

            }
            else
            {
                Announcer.Instance.Announce("Square Is Occupied By A Piece.");
            }
        }

        [PunRPC]
        public void RPC_DeployPiece(PieceIndex index, int file, int rank, Faction faction)
        {
            Piece newPiece = CreatePieceObject(index, faction);
            newPiece.transform.position = ArrayPosToWorldPos(_board[file, rank]);
            _board[file, rank].Piece = newPiece;
            OccupySquares(newPiece, _board[file, rank]);
        }

        public PieceData GetPieceDataFromIndex(PieceIndex index)
        {
            switch (index)
            {
                case PieceIndex.P:
                    return Pawn;
                case PieceIndex.N:
                    return Knight;
                case PieceIndex.B:
                    return Bishop;
                case PieceIndex.R:
                    return Rook;
                case PieceIndex.Q:
                    return Queen;
                case PieceIndex.K:
                    return King;
                default:
                    break;
            }
            Debug.LogWarning("Piece index not found: " + index);
            return null;
        }

        public Piece CreatePieceObject(PieceIndex index, Faction faction)
        {
            Piece newPiece = Instantiate(PiecePrefab);
            newPiece.Load(GetPieceDataFromIndex(index), faction);
            return newPiece;
        }

        private void OccupySquares(Piece piece, Square pieceSquare)
        {
            List<Square> legalMoves = piece.Data.FindLegalMoves(pieceSquare, this._board, piece.Faction);
            foreach (Square square in legalMoves)
            {
                square.Faction = piece.Faction;
                _squareViewArray[square.File, square.Rank].SetColor();
            }
        }

        public void DestroyPieceOnSquare(Square square)
        {
            if (square.Piece != null)
            {
                Destroy(square.Piece.gameObject);
                square.Piece = null;
            }
            else
            {
                Debug.LogWarning($"There's no piece on this square: {square.ToString()}");
            }
        }

        public int GetWhiteSquareCount()
        {
            return _board.Cast<Square>().Where(x => x.Faction == Faction.WHITE).Count();
        }

        public int GetBlackSquareCount()
        {
            return _board.Cast<Square>().Where(x => x.Faction == Faction.BLACK).Count();
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
                return this.transform.position + new Vector3(3 - file, 3 - rank);
            }
            return Vector3.zero;
        }

        private void GetComponents()
        {
            _pieceDeplCal = GetComponent<PieceDeployingCalculator>();
            _squareHighlighter = GetComponent<SquareHighlighter>();
        }

        public List<SquareView> GetSquareViewsFromSquareList(List<Square> squares)
        {
            List<SquareView> squareViews = new List<SquareView>();
            foreach (Square square in squares)
            {
                squareViews.Add(_squareViewArray[square.File, square.Rank]);
            }
            return squareViews;
        }

        public SquareView GetSquareView(Square square)
        {
            return _squareViewArray[square.File, square.Rank];
        }

        public PieceDeployingCalculator PieceDeplCal { get => _pieceDeplCal; }
        public SquareHighlighter SquareHighlighter { get => _squareHighlighter; }
        public Square[,] Board { get => _board; }
        public SquareView[,] SquareViewArray { get => _squareViewArray; }
    }

    public enum BoardViewMode
    {
        WHITE,
        BLACK
    }
}

