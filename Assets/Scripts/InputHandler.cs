using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class InputHandler : MonoBehaviour
    {
        public event Action Press;
        public event Action LongPressStart;
        public event Action LongPressEnd;

        [SerializeField] private float _maxPressTime = 1;

        private bool _isPressed = false;
        private bool _longPressActivated = false;
        private float _pressedTime = 0;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isPressed = true;
            }

            if (_isPressed)
            {
                _pressedTime += Time.deltaTime;
            }

            if (!_longPressActivated && _pressedTime > _maxPressTime)
            {
                _longPressActivated = true;
                LongPressStart?.Invoke();
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isPressed = false;

                if (_pressedTime <= _maxPressTime)
                {
                    Press?.Invoke();
                }
                else
                {
                    _longPressActivated = false;
                    LongPressEnd?.Invoke();
                }

                _pressedTime = 0;
            }
        }
    }
}
