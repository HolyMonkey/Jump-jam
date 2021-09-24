using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace JumpJam
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(IInputPresenter))]
    public class MonsterTruck : MonoBehaviour
    {
        [SerializeField] private bool _showConsumeText = false;
        [SerializeField] private RisingText _textPrefab = null;
        [SerializeField] private List<Transform> _rotationWheels = new List<Transform>();
        [SerializeField] private float _speed = 10;
        [SerializeField] private float _acceleration = 10;
        [SerializeField] private float _rotationSpeed = 10;
        [SerializeField] private float _maxRotationAngle = 45;
        [SerializeField] private MonsterTruckTrigger _trigger;

        private Rigidbody _rigidbody = null;
        private IInputPresenter _input = null;
        private Vector3 _currentDirection = Vector3.forward;
        private Vector3 _targetDirection = Vector3.zero;
        private float _currentSpeed = 0;
        private float _oldYPosition = 0;
        private bool _isStanned = false;
        private int _scoreToScale = 10;
        private bool _destroyed = false;
        private int _currentScore = 0;
        private int _currentSize = 1;
        private const string SizeUp = "Size Up!";

        public int CurrentSize => _currentSize;
        public UnityAction<int> SizeChanged;
        public UnityAction<MonsterTruck> Destroyed;

        public float Speed
        {
            get => Mathf.LerpUnclamped(_speed * 0.5f, _speed, transform.localScale.y / 3.0f);
        }

        public float Acceleration
        {
            get => Mathf.LerpUnclamped(_acceleration * 0.5f, _acceleration, transform.localScale.y / 3.0f);
        }

        public bool IsStanned 
        {
            get => _isStanned;
            set
            {
                if (_isStanned != value)
                    _isStanned = value;
            }
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _input = GetComponent<IInputPresenter>();
            _oldYPosition = _rigidbody.position.y;
        }

        private void Update()
        {
            _targetDirection = _input.GetCurrentInput();
            _targetDirection.z = _targetDirection.y;
            _targetDirection.y = 0;

            if (_targetDirection != Vector3.zero)
                _currentDirection = _targetDirection;
        }

        private void FixedUpdate()
        {
            var targetRotation = Quaternion.LookRotation(Quaternion.AngleAxis(-45, Vector3.up) * _currentDirection, Vector3.up);
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
                wheel.rotation = Quaternion.Slerp(wheel.rotation, Quaternion.AngleAxis(Mathf.Clamp(angleDiff, -_maxRotationAngle, _maxRotationAngle), Vector3.up) * _rigidbody.rotation, _rotationSpeed / 7 * Time.deltaTime);
            }

            if (IsStanned)
            {
                _currentSpeed = Mathf.Lerp(_currentSpeed, -_rigidbody.velocity.magnitude * 50, Acceleration * Time.deltaTime);
                return;
            }

            _currentSpeed = Mathf.MoveTowards(_currentSpeed, Speed, Acceleration * Time.deltaTime);

            _rigidbody.velocity = _rigidbody.rotation * (Vector3.forward * _currentSpeed * Time.deltaTime);

            var newPosition = _rigidbody.position;
            newPosition.y = _oldYPosition;
            _rigidbody.position = newPosition;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out DestroyTest obstacle))
            {
                OnDestroyObstacleColide(obstacle);
                return;
            }

            if (collision.gameObject.TryGetComponent(out Wall wall) || _rigidbody.velocity.y > 1.5f)
                return;

            if (collision.gameObject.TryGetComponent<MonsterTruck>(out MonsterTruck monsterTruck))
            {
                CheckCollision(monsterTruck);
            }

            if (collision.gameObject.TryGetComponent<Wheel>(out Wheel wheel))
            {
                CheckCollision(wheel.Truck);
            }

            IsStanned = false;
        }

        public void CheckCollision(MonsterTruck other)
        {
            if (_currentSize < other.CurrentSize)
            {
                gameObject.SetActive(false);
            }
            else if (_currentSize == other.CurrentSize)
            {
                gameObject.SetActive(false);
                other.gameObject.SetActive(false);
            }
        }

        private void OnDestroyObstacleColide(DestroyTest obstacle)
        {
            if (!obstacle.enabled)
                return;

            EnvironmentObject environmentObject = obstacle.GetComponent<EnvironmentObject>();

            if (environmentObject == null && obstacle.enabled == true)
            {
                obstacle.Destroy(transform.position);
                return;
            }

            int score = 0;

            if (environmentObject != null)
            {
                score = environmentObject.Reward;
                OnObjectDestroyed(score);
            }

            obstacle.Destroy(transform.position);
        }

        private void OnObjectDestroyed(int score)
        {
            if (score == 0)
                return;

            bool hasGrown = false;

            _currentScore += score;

            if (_currentScore >= _scoreToScale)
            {
                Vector3 newScale = transform.localScale * 1.25f;
                transform.DOScale(newScale, 0.5f);

                _currentSize++;
                SizeChanged?.Invoke(_currentSize);

                _scoreToScale *= 2;
                hasGrown = true;
            }

            if (_showConsumeText)
            {
                MakeRisingText(score.ToString(), Color.white);

                if (hasGrown)
                {
                    MakeRisingText(SizeUp, Color.red, true, 2, 10, 0.1f);
                }
            }
        }

        private void MakeRisingText(string text, Color color, bool useBoldStyle = false, float scaleMultiplier = 1, int layerOrder = 0, float outlineWidth = 0)
        {
            var x = Random.value * 4f;
            var z = Random.value * 4f;
            var position = Vector3.Scale(new Vector3(x - 2, 3, z - 2), transform.lossyScale);

            var _text = Instantiate(_textPrefab);

            _text.SetText(text);
            _text.SetColor(color);

            if (useBoldStyle)
                _text.ToggleFontStyleBold();

            _text.SetSortingOrder(layerOrder);
            _text.SetOutlineWidth(outlineWidth);

            _text.transform.position = position + transform.position;
            _text.transform.localScale = Vector3.Scale(_text.transform.localScale, transform.lossyScale / 2) * scaleMultiplier;
        }

        private void OnDisable()
        {
            Destroyed?.Invoke(this);
        }
    }
}