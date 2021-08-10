using DG.Tweening;
using RayFire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class DestroyTest : MonoBehaviour
    {
        private Rigidbody _rigidbody = null;
        private RayfireRigid _rayfireRigid = null;
        private RigidbodyActivator _rigidbodyActivator = null;

        private void Awake()
        {
            TryInitDependencies();
        }

        public void Destroy(Vector3 position)
        {
            if (!enabled)
            {
                return;
            }
            enabled = false;

            if (_rayfireRigid != null)
            {
                _rayfireRigid.Activate();
                _rayfireRigid.Demolish();
                return;
            }

            if (_rigidbodyActivator != null)
            {
                _rigidbodyActivator.Activate();
                return;
            }

            if (_rigidbody != null)
            {
                _rigidbody.isKinematic = false;
                transform.DOScale(0, 2).SetDelay(1 + (Random.value - 0.5f)).OnComplete(OnScaleCompleted);
                return;
            }

            Destroy(gameObject);
        }

        private void OnScaleCompleted()
        {
            Destroy(gameObject);
        }

        private void TryInitDependencies()
        {
            if (TryGetComponent(out _rayfireRigid) && _rayfireRigid.enabled)
                _rayfireRigid = GetComponent<RayfireRigid>();

            TryInitRigidbody();
        }

        private void TryInitRigidbody()
        {
            if (TryGetComponent(out _rigidbody))
            {
                _rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
                _rigidbody.isKinematic = true;
            }

            _rigidbodyActivator = GetComponent<RigidbodyActivator>();
        }

        public void InitRayfireRigid()
        {
            _rayfireRigid.Initialize();

            TryInitRigidbody();
        }
    }
}
