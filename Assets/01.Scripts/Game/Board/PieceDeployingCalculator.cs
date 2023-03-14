using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    public class PieceDeployingCalculator : MonoBehaviour
    {
        private BoardView _boardView;
        private PieceData _piece;
        private Faction _faction;

        private void Awake()
        {
            _boardView = GetComponent<BoardView>();
        }

        public void DeployPieceActionStarted(PieceIndex index, Faction faction)
        {
            _faction = faction;
            _piece = _boardView.GetPieceDataFromIndex(index);
            SquareEventList.Instance.SquareHovered.OnEventRaised += SquareHovered;
        }

        public void DeployPieceActionEnded()
        {
            SquareEventList.Instance.SquareHovered.OnEventRaised -= SquareHovered;
        }

        private void SquareHovered(Square square)
        {
            var attackedSquares = CalculateAttackedSquares(_piece, square);
            _boardView.SquareHighlighter.Highlight(_boardView.GetSquareViewsFromSquareList(attackedSquares));
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
