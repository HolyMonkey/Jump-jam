using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JumpJam
{
    public class TimeCounter : MonoBehaviour
    {
        [SerializeField] private float _limitInSeconds;

        private float _elapsedTime = 0;

        public event UnityAction Stopped;

        private void Awake()
        {
            Time.timeScale = 1;
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _limitInSeconds)
            {
                Stopped?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}
