using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace JumpJam
{
    [RequireComponent(typeof(TMP_Text))]
    public class TimeCounter : MonoBehaviour
    {
        [SerializeField] private float _limitInSeconds;

        private TMP_Text _timer;
        private int _secondsRemain;
        private int _previousTime;

        public event UnityAction Stopped;

        private void Awake()
        {
            Time.timeScale = 1;

            _timer = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            _limitInSeconds -= Time.deltaTime;
            _secondsRemain = (int)_limitInSeconds % 60;

            if (_secondsRemain != _previousTime)
            {
                _timer.text = _secondsRemain.ToString();
                _previousTime = _secondsRemain;
            }

            if (_limitInSeconds <= 0)
            {
                Stopped?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}
