using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace JumpJam
{
    [RequireComponent(typeof(TextMeshPro))]
    public class RisingText : MonoBehaviour
    {
        [SerializeField] private float _speed = 1;

        private TextMeshPro _textMesh = null;

        private void Awake()
        {
            _textMesh = GetComponent<TextMeshPro>();
            StartCoroutine(StartFading());
        }

        private void Update()
        {
            transform.position += Vector3.up * (_speed * transform.localScale.y) * Time.deltaTime;
            transform.LookAt(Camera.main.transform);
        }
        
        private IEnumerator StartFading()
        {
            _textMesh.DOFade(1, 1).From(0);
            yield return new WaitForSeconds(1);
            _textMesh.DOFade(0, 1);
            yield return new WaitForSeconds(1);
            Destroy(gameObject);
        }

        public void SetText(string text)
        {
            _textMesh.text = text;
        }

        public void SetColor(Color color)
        {
            _textMesh.color = color;
        }

        public void ToggleFontStyleBold()
        {
            _textMesh.fontStyle ^= FontStyles.Bold;
        }

        public void SetFontSize(float fontSize)
        {
            _textMesh.fontSize = fontSize;
        }

        public void SetSortingOrder(int sortingOrder)
        {
            _textMesh.sortingOrder = sortingOrder;
        }

        public void SetOutlineWidth(float width)
        {
            _textMesh.outlineWidth = width;
        }
    }
}
