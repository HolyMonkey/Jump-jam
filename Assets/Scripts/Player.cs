using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JumpJam
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private Joystick _joystick = null;
        //[SerializeField] private List<Transform> _rotationWheels = new List<Transform>();
        [SerializeField] private float _speed = 10;
        [SerializeField] private float _rotationSpeed = 10;

        private Rigidbody _rigidbody = null;
        private Vector3 _currentDirection = Vector3.zero;
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
            //_rigidbody.rotation = Quaternion.RotateTowards(_rigidbody.rotation, Quaternion.LookRotation(_currentDirection, Vector3.up), _rotationSpeed * Time.deltaTime);
            _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, Quaternion.LookRotation(_currentDirection, Vector3.up), _rotationSpeed * Time.deltaTime);

            _rigidbody.velocity = Vector3.Slerp(_rigidbody.velocity, _rigidbody.rotation * new Vector3(0, 0, _speed * Time.deltaTime), _rotationSpeed * Time.deltaTime);
        }
    }
}