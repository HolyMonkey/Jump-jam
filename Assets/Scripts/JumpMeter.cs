using JumpJam.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JumpJam
{
    public class JumpMeter : MonoBehaviour
    {
        public event Action<float> End;

        [SerializeField] private InputHandler _input = null;
        [SerializeField] private Image _background = null;
        [SerializeField] private Image _foreground = null;
        [SerializeField] private float _min = 0;
        [SerializeField] private float _max = 1;

        private bool _isIncreasing = true;
        private bool _isEnabled = false;
        private float _value = 0;

        public float Value => (_value - _min) / (_max - _min);

        private void OnEnable()
        {
            _input.LongPressStart += OnStarted;
            _input.LongPressEnd += OnEnded;
        }

        private void OnDisable()
        {
            _input.LongPressStart -= OnStarted;
            _input.LongPressEnd -= OnEnded;
        }

        private void Update()
        {
            if (!_isEnabled)
            {
                return;
            }

            if (_isIncreasing)
            {
                _value += Time.deltaTime;

                if (_value > _max)
                {
                    _isIncreasing = false;
                }
            }
            else
            {
                _value -= Time.deltaTime;

                if (_value < _min)
                {
                    _isIncreasing = true;
                }
            }

            _foreground.fillAmount = EasingFunctions.EaseInQuart(0, 1, _value / _max);
        }

        private void OnStarted()
        {
            _value = 0;
            _isEnabled = true;
            _background.enabled = true;
            _foreground.enabled = true;
        }

        private void OnEnded()
        {
            _isEnabled = false;
            _background.enabled = false;
            _foreground.enabled = false;

            End?.Invoke(Value);
        }
    }
}
