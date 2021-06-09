using JumpJam.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class Coin : MonoBehaviour, ICollectable
    {
        public event Action<Coin> GenerationRequested;

        [SerializeField] private Renderer _renderer = null;

        private void Update()
        {
            if (Camera.main.transform.position.z > transform.position.z)
            {
                GenerationRequested?.Invoke(this);
            }
        }

        public void Collect()
        {
            GenerationRequested?.Invoke(this);
        }

        public void SetMaterial(Material mat)
        {
            _renderer.material = mat;
        }
    }
}
