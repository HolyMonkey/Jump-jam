using DG.Tweening;
using RayFire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class DestroyTest : MonoBehaviour
    {
        private readonly WaitForEndOfFrame _delayBetweenCheckingFragments = new WaitForEndOfFrame();

        private Rigidbody _rigidbody = null;
        private RayfireRigid _rayfireRigid = null;
        private RigidbodyActivator _rigidbodyActivator = null;

        private void Awake()
        {
            if (TryGetComponent(out _rayfireRigid) && _rayfireRigid.enabled)
            {
                _rayfireRigid.Initialize();
                _rayfireRigid.physics.meshCollider.isTrigger = true;
            }
            if (TryGetComponent(out _rigidbody))
            {
                _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                _rigidbody.isKinematic = true;
            }
            _rigidbodyActivator = GetComponent<RigidbodyActivator>();
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
                //if (!_rayfireRigid.DemolitionState())
                //{
                //    _rayfireRigid.Demolish();
                //}
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

            //_rigid.StartCoroutine(AddForceAfterFragmentsInstantinated(position));
            //_rigid.Activate();
            //_rigid.Demolish();
            /*var colliders = Physics.OverlapSphere(position, 10);
            foreach (var collider in colliders)
            {
                if (collider.attachedRigidbody != null)
                {
                    collider.attachedRigidbody.AddExplosionForce(100, position, 100);
                }
            }*/
        }

        private void OnScaleCompleted()
        {
            Destroy(gameObject);
        }
    }
}
