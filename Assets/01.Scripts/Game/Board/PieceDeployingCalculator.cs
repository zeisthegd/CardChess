using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    public class PieceDeployingCalculator : MonoBehaviour
    {
        private BoardView _boardView;
        private PieceData _pieceData;
        private Faction _faction;
        private Piece _ghostPiece;

        private void Awake()
        {
            _boardView = GetComponent<BoardView>();
            _ghostPiece = _boardView.CreatePieceObject(PieceIndex.P, Faction.WHITE);
            _ghostPiece.GetComponent<SpriteRenderer>().color *= new Color(1, 1, 1, 0.5f);
            _ghostPiece.gameObject.SetActive(false);
        }

        public void DeployPieceActionStarted(PieceIndex index, Faction faction)
        {
            _faction = faction;
            _pieceData = _boardView.GetPieceDataFromIndex(index);
            _ghostPiece.gameObject.SetActive(true);
            _ghostPiece.Load(_pieceData, faction);
            SquareEventList.Instance.SquareHovered.OnEventRaised += SquareHovered;
        }

        public void DeployPieceActionEnded()
        {
            _ghostPiece.gameObject.SetActive(false);
            _boardView.SquareHighlighter.UnhighlightAll();
            SquareEventList.Instance.SquareHovered.OnEventRaised -= SquareHovered;
        }

        private void SquareHovered(Square square)
        {
            var attackedSquares = CalculateAttackedSquares(_pieceData, square);
            _boardView.SquareHighlighter.Highlight(_boardView.GetSquareViewsFromSquareList(attackedSquares));
            _ghostPiece.transform.position = _boardView.GetSquareView(square).transform.position;
        }

        /// <summary>
        /// Get squares that will be faction-changed when the piece is deployed.
        /// </summary>
        /// <returns></returns>
        private List<Square> CalculateAttackedSquares(PieceData piece, Square squareToDeploy)
        {
            List<Square> result = new List<Square>();
            if (squareToDeploy.Piece == null)
            {
                //TODO Actual calculation algorithm
                result = piece.FindLegalMoves(squareToDeploy, _boardView.Board, _faction);
            }
            return result;
        }



    }

}
