using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class Wheel : MonoBehaviour
    {
        [SerializeField] private WheelCollider _collider = null;

        public float SteerAngle { get => _collider.steerAngle; set => _collider.steerAngle = value; }
        public float MotorTorque { get => _collider.motorTorque; set => _collider.motorTorque = value; }
        public float RPM => _collider.rpm;

        private void Update()
        {
            _collider.GetWorldPose(out var position, out var rotation);

            transform.position = position;
            transform.rotation = rotation;
        }
    }
}
