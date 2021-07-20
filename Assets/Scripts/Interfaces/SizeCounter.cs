using TMPro;
using UnityEngine;

namespace JumpJam
{
    [RequireComponent(typeof(TMP_Text))]
    public class SizeCounter : MonoBehaviour
    {
        [SerializeField] private MonsterTruck _player;

        private TMP_Text _counter;

        private void Awake()
        {
            _counter = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            _player.SizeChanged += OnPlayerSizeChanged;
        }

        private void OnDisable()
        {
            _player.SizeChanged -= OnPlayerSizeChanged;
        }

        private void OnPlayerSizeChanged(int size)
        {
            _counter.text = size.ToString();
        }
    }
}