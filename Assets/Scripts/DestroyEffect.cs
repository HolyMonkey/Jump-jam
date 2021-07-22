using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class DestroyEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _effectPrefab = null;
        [SerializeField] private bool _useObjectScale = false;
        [SerializeField] private float _scaleMultiplier = 1;
        [SerializeField] private float _shakingDuration = 1;
        [SerializeField] private float _shakingStrength = 1;

        private void OnDestroy()
        {
            if (enabled == false)
            {
                var effect = Instantiate(_effectPrefab, transform.position, transform.rotation);

                if (_useObjectScale)
                {
                    effect.transform.localScale = transform.lossyScale * _scaleMultiplier;
                    var main = effect.main;
                    main.scalingMode = ParticleSystemScalingMode.Hierarchy;
                }

                CameraShake.AddShake(_shakingDuration, _shakingStrength);
            }
        }
    }
}
