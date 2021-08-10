using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(MeshRenderer))]
    public class EnvironmentObject : MonoBehaviour
    {
        [SerializeField] private int _level;
        [SerializeField] private int _reward;
        [SerializeField] private Material[] _transparentMaterials;

        private Collider _collider;
        private MeshRenderer _renderer;
        private Material[] _materials;
        private Material[] _opaqueMaterials;
        private float _secondsToChangeMaterial = 0.3f;

        public int Level => _level;
        public int Reward => _reward;

        private void Start()
        {
            TryGetComponent<Collider>(out Collider collider);

            if (collider != null)
                _collider = GetComponent<Collider>();

            _renderer = GetComponent<MeshRenderer>();

            _materials = _renderer.materials;

            _opaqueMaterials = _renderer.materials;
        }

        public void TurnOnTriggerCollider()
        {
            _collider.isTrigger = true;

            if (_renderer == null)
                return;

            ApplyTransparency();
        }

        public void TurnOffTriggerCollider()
        {
            _collider.isTrigger = false;

            if (_renderer == null)
                return;

            ApplyOpaque(_secondsToChangeMaterial);
        }

        public bool CheckTriggerIsOn()
        {
            return _collider.isTrigger;
        }

        private void ApplyTransparency()
        {
            for (int i = 0; i < _materials.Length; i++)
            {
                _materials[i] = _transparentMaterials[i];
            }

            _renderer.materials = _materials;
        }

        private void ApplyOpaque(float time)
        {
            StartCoroutine(ApplyOpaqueMaterial(time));
        }

        private IEnumerator ApplyOpaqueMaterial(float time)
        {
            var waitForSeconds = new WaitForSeconds(time);

            yield return waitForSeconds;

            for (int i = 0; i < _materials.Length; i++)
            {
                _materials[i] = _opaqueMaterials[i];
            }

            _renderer.materials = _materials;
        }
    }
}
