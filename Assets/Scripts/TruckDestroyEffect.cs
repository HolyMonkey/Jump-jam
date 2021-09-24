using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class TruckDestroyEffect : MonoBehaviour
    {
        [SerializeField] private MonsterTruck _truck;
        [SerializeField] private ParticleSystem _effectPrefab = null;
        [SerializeField] private bool _useObjectScale = false;
        [SerializeField] private float _scaleMultiplier = 1;
        [SerializeField] private float _shakingDuration = 1;
        [SerializeField] private float _shakingStrength = 1;

        private void OnEnable()
        {
            _truck.Destroyed += OnTruckDestroyed;
        }

        private void OnDisable()
        {
            _truck.Destroyed -= OnTruckDestroyed;
        }

        public void OnTruckDestroyed(MonsterTruck truck)
        {
            var effect = Instantiate(_effectPrefab, _truck.transform.position, _truck.transform.rotation);

            if (_useObjectScale)
            {
                effect.transform.localScale = _truck.transform.lossyScale * _scaleMultiplier;
                var main = effect.main;
                main.scalingMode = ParticleSystemScalingMode.Hierarchy;
            }

            if (_truck.TryGetComponent<Player>(out Player player))
                CameraShake.AddShake(_shakingDuration, _shakingStrength);
        }
    }
}
