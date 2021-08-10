using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    [RequireComponent(typeof(MeshRenderer))]
    public class TransparencyChanger : MonoBehaviour
    {
        [SerializeField] private MonsterTruckTrigger _trigger;
        [SerializeField] private Material _transparentMaterial;

        private MeshRenderer _renderer;
        private Material _opaqueMaterial;

        private void Start()
        {
            _renderer = GetComponent<MeshRenderer>();
            _opaqueMaterial = _renderer.material;
        }

        private void OnEnable()
        {
            _trigger.TriggerChanged += OnTriggerChanged;
        }

        private void OnDisable()
        {
            _trigger.TriggerChanged -= OnTriggerChanged;
        }

        private void OnTriggerChanged(bool isTrigger)
        {
            if (isTrigger)
                ApplyTransparency();
            else
                ApplyOpaque();
        }

        private void ApplyTransparency()
        {
            _renderer.material = _transparentMaterial;
        }

        private void ApplyOpaque()
        {
            _renderer.material = _opaqueMaterial;
        }
    }
}
