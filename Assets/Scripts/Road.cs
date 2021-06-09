using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class Road : MonoBehaviour
    {
        public event Action GenerationRequested;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player>(out _))
            {
                GenerationRequested?.Invoke();
            }
        }
    }
}
