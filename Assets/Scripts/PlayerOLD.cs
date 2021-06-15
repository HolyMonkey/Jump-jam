using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JumpJam
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerOLD : MonoBehaviour
    {
        [SerializeField] private Joystick _joystick = null;
        [SerializeField] private List<Transform> _rotationWheels = new List<Transform>();
        [SerializeField] private float _speed = 10;
        [SerializeField] private float _acceleration = 10;
        [SerializeField] private float _rotationSpeed = 10;
        [SerializeField] private float _maxRotationAngle = 45;

        private Rigidbody _rigidbody = null;
        private Vector3 _currentDirection = Vector3.forward;
        private Vector3 _targetDirection = Vector3.zero;
        private float _currentSpeed = 0;

        public bool IsStanned { get; set; }

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
            var targetRotation = Quaternion.LookRotation(Quaternion.AngleAxis(-45, Vector3.up) * _currentDirection, Vector3.up);
            //_rigidbody.rotation = Quaternion.RotateTowards(_rigidbody.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            var val1 = Quaternion.RotateTowards(_rigidbody.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            var val2 = Quaternion.Slerp(_rigidbody.rotation, targetRotation, _rotationSpeed / 10 * Time.deltaTime);
            _rigidbody.rotation = Quaternion.Angle(val1, val2) < _rotationSpeed * Time.deltaTime ? val2 : val1;

            var forwardA = _rigidbody.rotation * Vector3.forward;
            var forwardB = targetRotation * Vector3.forward;

            var angleA = Mathf.Atan2(forwardA.x, forwardA.z) * Mathf.Rad2Deg;
            var angleB = Mathf.Atan2(forwardB.x, forwardB.z) * Mathf.Rad2Deg;

            var angleDiff = Mathf.DeltaAngle(angleA, angleB);

            foreach (var wheel in _rotationWheels)
            {
                //wheel.transform.rotation = Quaternion.Slerp(wheel.transform.rotation, targetRotation, _rotationSpeed * 4 * Time.deltaTime);
                //var newWheelRot = wheel.transform.eulerAngles;
                //newWheelRot.y = Mathf.LerpAngle(wheel.transform.eulerAngles.y, Mathf.Clamp(angleDiff, -_maxRotationAngle, _maxRotationAngle), _rotationSpeed * 2 * Time.deltaTime);
                //wheel.transform.eulerAngles = newWheelRot;
                wheel.rotation = Quaternion.Slerp(wheel.rotation, Quaternion.AngleAxis(Mathf.Clamp(angleDiff, -_maxRotationAngle, _maxRotationAngle), Vector3.up) * _rigidbody.rotation, _rotationSpeed / 10 * Time.deltaTime);
            }

            if (IsStanned)
            {
                _currentSpeed = Mathf.Lerp(_currentSpeed, -_rigidbody.velocity.magnitude * 50, _acceleration * Time.deltaTime);
                return;
            }

            _currentSpeed = Mathf.MoveTowards(_currentSpeed, _speed, _acceleration * Time.deltaTime);

            _rigidbody.velocity = _rigidbody.rotation * new Vector3(0, 0, _currentSpeed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Wall wall))
            {
                return;
            }

            IsStanned = false;
        }
    }
}