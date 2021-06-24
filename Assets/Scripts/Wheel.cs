using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class Wheel : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rootBody = null;
        //[SerializeField] private SphereCollider _collider = null;

        //public float SteerAngle { get => _collider.steerAngle; set => _collider.steerAngle = value; }
        //public float MotorTorque { get => _collider.motorTorque; set => _collider.motorTorque = value; }
        //public float RPM => _collider.rpm;

        private void Update()
        {
            /*
            if (_collider == null || !_collider.gameObject.activeInHierarchy)
            {
                return;
            }

            _collider.GetWorldPose(out var position, out var rotation);

            transform.position = position;
            transform.rotation = rotation;
            */

            //transform.Rotate(_rootBody.transform.right * -_rootBody.velocity.x, Space.Self);

            var temp = new Vector2(-_rootBody.velocity.x, _rootBody.velocity.z);
            transform.Rotate(Vector3.right * (temp.magnitude / 2), Space.Self);
        }
    }
}
