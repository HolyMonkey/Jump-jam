using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class ScoreBoard : MonoBehaviour
    {
        [SerializeField] private List<ScoreCounter> _elements = new List<ScoreCounter>();

        private void OnEnable()
        {
            for (int i = 0; i < _elements.Count; i++)
            {
                _elements[i].ScoreChanged += OnScoreChanged;
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _elements.Count; i++)
            {
                _elements[i].ScoreChanged -= OnScoreChanged;
            }
        }

        private void OnScoreChanged(RectTransform element, int score)
        {
            int hierarchyCurrentIndex = element.GetSiblingIndex();

            int hierarchyElementIndex;
            int hierarchyIndex;

            for (int i = hierarchyCurrentIndex; i >= 0; i--)
            {
                if (_elements[i].Score < score)
                {
                    RectTransform other = _elements[i].GetComponent<RectTransform>();
                    hierarchyIndex = other.GetSiblingIndex();
                    hierarchyElementIndex = element.GetSiblingIndex();
                    element.SetSiblingIndex(hierarchyIndex);
                    other.SetSiblingIndex(hierarchyElementIndex);

                    int listElementIndex = _elements.IndexOf(element.GetComponent<ScoreCounter>());
                    int listIndex = _elements.IndexOf(_elements[i]);

                    SwapListElements(listElementIndex, listIndex);
                }
            }
        }

        private void SwapListElements(int elementIndex, int listIndex)
        {
            ScoreCounter scoreBoard = _elements[elementIndex];
            _elements[elementIndex] = _elements[listIndex];
            _elements[listIndex] = scoreBoard;
        }
    }
}
