using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace JumpJam
{
    [RequireComponent(typeof(RectTransform))]
    public class ScoreCounter : MonoBehaviour
    {
        [SerializeField] private MonsterTruck _monsterTruck;
        [SerializeField] private TMP_Text _counter;
        [SerializeField] private TMP_Text _label;

        private RectTransform _rectTransform;
        private int _score;

        public event UnityAction<RectTransform, int> ScoreChanged;
        public int Score => _score;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            _monsterTruck.SizeChanged += OnSizeChanged;
            _monsterTruck.Destroyed += OnTruckDestroyed;
        }

        private void OnDisable()
        {
            _monsterTruck.SizeChanged -= OnSizeChanged;
            _monsterTruck.Destroyed -= OnTruckDestroyed;
        }

        private void OnSizeChanged(int size)
        {
            _score = size;
            _counter.text = size.ToString();
            ScoreChanged?.Invoke(_rectTransform, size);
        }

        private void OnTruckDestroyed()
        {
            if (_monsterTruck.GetComponent<Player>() == null)
                gameObject.SetActive(false);
        }
    }
}
