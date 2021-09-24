using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JumpJam
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerNew : MonoBehaviour
    {
        [SerializeField] private Joystick _joystick = null;
        [SerializeField] private List<WheelNew> _rotationWheels = new List<WheelNew>();
        [SerializeField] private List<WheelNew> _allWheels = new List<WheelNew>();
        [SerializeField] private float _speed = 10;
        [SerializeField] private float _rotationSpeed = 10;
        [SerializeField] private float _maxRotationAngle = 45;

        private Rigidbody _rigidbody = null;
        private Vector3 _currentDirection = Vector3.forward;
        private Vector3 _targetDirection = Vector3.zero;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _targetDirection.x = _joystick.Horizontal;
            _targetDirection.z = _joystick.Vertical;

            if (_targetDirection != Vector3.zero)
            {
                _currentDirection = _targetDirection;
            }
        }

        private void FixedUpdate()
        {
            _rigidbody.angularVelocity.Set(0, _rigidbody.angularVelocity.y, 0);

            //_rigidbody.rotation = Quaternion.RotateTowards(_rigidbody.rotation, Quaternion.LookRotation(_currentDirection, Vector3.up), _rotationSpeed * Time.deltaTime);
            var targetRotation = Quaternion.LookRotation(Quaternion.AngleAxis(-45, Vector3.up) * _currentDirection, Vector3.up);
            //_rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            var forwardA = _rigidbody.rotation * Vector3.forward;
            var forwardB = targetRotation * Vector3.forward;

            var angleA = Mathf.Atan2(forwardA.x, forwardA.z) * Mathf.Rad2Deg;
            var angleB = Mathf.Atan2(forwardB.x, forwardB.z) * Mathf.Rad2Deg;

            var angleDiff = Mathf.DeltaAngle(angleA, angleB);

            foreach (var wheel in _rotationWheels)
            {
                //wheel.rotation = Quaternion.Slerp(wheel.rotation, targetRotation, _rotationSpeed * 4 * Time.deltaTime);
                wheel.SteerAngle = Mathf.LerpAngle(wheel.SteerAngle, Mathf.Clamp(angleDiff, -_maxRotationAngle, _maxRotationAngle), _rotationSpeed * 2 * Time.deltaTime);
            }

            //_rigidbody.velocity = _rigidbody.rotation * new Vector3(0, 0, _speed * Time.deltaTime);

            foreach (var wheel in _allWheels)
            {
                var speed = _speed * Time.deltaTime;

                if (wheel.RPM > 500)
                {
                    speed = 0;
                }

                if (_currentDirection.magnitude < 0.25f)
                {
                    speed = -speed;
                }

                wheel.MotorTorque = speed;
            }
        }
    }
}