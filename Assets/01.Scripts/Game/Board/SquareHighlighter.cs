using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    public class SquareHighlighter : MonoBehaviour
    {
        public ObjectPooler HighlighterPooler;
        private BoardView _boardView;

        private void Awake()
        {
            _boardView = GetComponent<BoardView>();
        }

        public void Highlight(List<SquareView> squareViewList)
        {
            UnhighlightAll();
            foreach (SquareView squareView in squareViewList)
            {
                Highlight(squareView);
            }
        }

        public void Highlight(SquareView squareView)
        {
            var obj = HighlighterPooler.PullOneObject();
            obj.transform.position = squareView.transform.position;
            obj.gameObject.SetActive(true);
        }

        public void UnhighlightAll()
        {
            HighlighterPooler.DisableAllPooledObjects();
        }

    }
}

