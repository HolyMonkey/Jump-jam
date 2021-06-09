using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class Wheel : MonoBehaviour
    {
        //[SerializeField] private Rigidbody _rootBody = null;
        [SerializeField] private WheelCollider _collider = null;

        private float _previousZPosition = 0;

        private void Start()
        {
            _previousZPosition = transform.position.z;
        }

        private void Update()
        {
            transform.rotation *= Quaternion.Euler(Vector3.right * (transform.position.z * _previousZPosition));

            _previousZPosition = transform.position.z;

            _collider.GetWorldPose(out Vector3 position, out Quaternion _);
            transform.position = position;
            //transform.rotation = rotation;
        }
    }
}
