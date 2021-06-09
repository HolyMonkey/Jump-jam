using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    [RequireComponent(typeof(Rigidbody))]
    public class CustomCenterOfMass : MonoBehaviour
    {
        [SerializeField] private Vector3 _targetCenterOfMass = Vector3.zero;

        private Rigidbody _rigidbody = null;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();

            _rigidbody.centerOfMass = _targetCenterOfMass;
        }

        private void OnValidate()
        {
            if (_rigidbody == null)
            {
                return;
            }

            _rigidbody.centerOfMass = _targetCenterOfMass;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position + _targetCenterOfMass, 0.1f);
        }
    }
}
