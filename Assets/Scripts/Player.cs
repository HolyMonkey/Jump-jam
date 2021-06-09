using DG.Tweening;
using JumpJam.Interfaces;
using JumpJam.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JumpJam
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        private readonly WaitForSeconds _delayBetweenAddingSpeed = new WaitForSeconds(20);

        [SerializeField] private InputHandler _input = null;
        [SerializeField] private JumpMeter _meter = null;
        [SerializeField] private List<Transform> _rotationWheels = new List<Transform>();
        [SerializeField] private float _playerSpeed = 10;
        [SerializeField] private float _jumpHeight = 2;
        [SerializeField] private float _animationsSpeed = 1;
        [SerializeField] private float _xPosition = 2.5f;

        private Rigidbody _rigidbody = null;
        private Sequence _togglingSequence = null;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            StartCoroutine(AddSpeed());
        }

        private void OnEnable()
        {
            _input.Press += ToggleLane;
            _meter.End += Jump;
        }

        private void OnDisable()
        {
            _input.Press -= ToggleLane;
            _meter.End -= Jump;
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, _playerSpeed);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollectable collectable))
            {
                collectable.Collect();
            }
        }

        public void ToggleLane()
        {
            if (_togglingSequence != null)
            {
                return;
            }

            _togglingSequence = DOTween.Sequence();

            var targetX = _rigidbody.position.x * -1 >= 0 ? _xPosition : -_xPosition;
            //_togglingSequence.Join(transform.DORotateQuaternion(Quaternion.Euler(0, targetX >= 0 ? 25 : -25, 0), _animationsSpeed).SetEase(RotationEasing));
            _togglingSequence.Join(_rigidbody.DORotate(new Vector3(0, targetX >= 0 ? 25 : -25, 0), _animationsSpeed).SetEase(RotationEasing).SetUpdate(UpdateType.Fixed));

            foreach (var wheel in _rotationWheels)
            {
                var targetRotation = Quaternion.Euler(0, targetX >= 0 ? 75 : -75, 0);
                _togglingSequence.Join(wheel.transform.DORotateQuaternion(targetRotation, _animationsSpeed).SetEase(RotationEasing));
            }
            
            _togglingSequence.Join(_rigidbody.DOMoveX(targetX, _animationsSpeed).SetEase(Ease.InOutCubic).SetUpdate(UpdateType.Fixed));

            _togglingSequence.onComplete = OnTogglingComplete;
        }

        public void Jump(float strength)
        {
            _rigidbody.AddForce(Vector3.up * _jumpHeight * strength, ForceMode.Impulse);

            //return;

            //var curve = new AnimationCurve();
            //curve.AddKey(new Keyframe(0, 0));
            //curve.AddKey(new Keyframe(0.5f, 1));
            //curve.AddKey(new Keyframe(1, 0));

            //transform.DOMoveY(transform.position.y + _jumpHeight, _animationsSpeed * 5).SetEase(curve);
        }

        private float RotationEasing(float time, float duration, float overshootOrAmplitude, float period)
        {
            var value = time * 2 / duration;

            if (value > 1)
            {
                return EasingFunctions.EaseOutCubic(1, 0, value - 1);
            }

            return EasingFunctions.EaseInCubic(0, 1, value);
        }

        private IEnumerator AddSpeed()
        {
            while (true)
            {
                yield return _delayBetweenAddingSpeed;

                _playerSpeed += 1;
            }
        }

        private void OnTogglingComplete()
        {
            _togglingSequence = null;
        }
    }
}