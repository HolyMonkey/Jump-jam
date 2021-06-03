using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarDrive : MonoBehaviour
{
    [SerializeField] private GameObject _carModel;
    [SerializeField] private Axle[] _carAxis;
    [SerializeField] private WheelCollider[] _wheelColliders;
    [SerializeField] private float _carSpeed;
    [SerializeField] private Transform _centerOfMass;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private UiPanel _uiPanel;
    [SerializeField] private GameObject _result;
    [Range(0, 1)] [SerializeField] private float _steerAssistValue;

    [SerializeField] private Effects _effects;
    [SerializeField] private Transmission _transmission;
    [SerializeField] private float _transmissionModifier;
    [SerializeField] private float _speed;
    private float _distance = 32;
    private float _steerAngle = 30;
    private bool _isGrounded;
    private float _previousYRotation;
    private bool _jumped = false;
    private float _startSpeed;
    private float _needlePositionModifier;
    private float _currentMaxTransitionModifier;
    public float MaxSpeed;

    [SerializeField] private float _currentAngle;
    [SerializeField] private int _cones = 0;
    [SerializeField] private int _checkpoints = 0;
    [SerializeField] private int _obstacles = 0;
    [SerializeField] private bool _wall = false;
    [SerializeField] private Joystick _joystick;

    public event UnityAction<int, int, int> StatisticChanged;

    public float CurrentAngle => _currentAngle;    
    public int Checkpoints => _checkpoints;
    public int Obstacles => _obstacles;    
        
    public bool IsGrounded => _isGrounded;
    public float CarSpeed => _carSpeed;
    public bool Jumped => _jumped;

    public float Distance => _distance;    


    private void OnEnable()
    {
        _transmission.SpeedChanged += OnSpeedChanged;
    }

    private void FixedUpdate()
    {
        if (_transmission.CurrentSpeed > 0)
        {
            _distance = 32 + transform.position.z;
        }
        else
        {
            _distance = 0;
        }


        StatisticChanged?.Invoke(Mathf.RoundToInt(_distance), _obstacles, _checkpoints);
        CheckGround();
        Accelerate();
        SteerAssist();
        LoseSpeed();
    }

    private void Start()
    {
        _startSpeed = _carSpeed;
        _rigidbody.centerOfMass = _centerOfMass.localPosition;
    }

    private void OnSpeedChanged(float value, int mod)
    {
        _rigidbody.AddForce(_carModel.transform.forward * _currentMaxTransitionModifier * 2000 * value, ForceMode.Impulse);

        if (value < 0)
        {
            Brake(50);
            Debug.Log(value);
        }
    }

    private void Accelerate()
    {
        if (!Jumped)
        {
            switch (_transmission.CurrentSpeed)
            {
                case 0:
                    _transmissionModifier = 0;
                    MaxSpeed = 0;
                    break;
                case 1:
                    _currentMaxTransitionModifier = 0.5f;
                    _transmissionModifier = Mathf.Lerp(_transmissionModifier, _currentMaxTransitionModifier, Time.fixedDeltaTime * 1.5f);
                    break;
                case 2:
                    _currentMaxTransitionModifier = 1f;
                    _transmissionModifier = Mathf.Lerp(_transmissionModifier, _currentMaxTransitionModifier, Time.fixedDeltaTime * 1.5f);
                    break;
                case 3:
                    _currentMaxTransitionModifier = 1.5f;
                    _transmissionModifier = Mathf.Lerp(_transmissionModifier, _currentMaxTransitionModifier, Time.fixedDeltaTime * 1.5f);
                    break;
                case 4:
                    _currentMaxTransitionModifier = 2f;
                    _transmissionModifier = Mathf.Lerp(_transmissionModifier, _currentMaxTransitionModifier, Time.fixedDeltaTime * 1.5f);
                    break;
                case 5:
                    _currentMaxTransitionModifier = 2.5f;
                    _transmissionModifier = Mathf.Lerp(_transmissionModifier, _currentMaxTransitionModifier, Time.fixedDeltaTime * 1.5f);
                    break;
            }
        }
        else
        {
            LoseSpeed();
           
        }

        MaxSpeed = _speed * _currentMaxTransitionModifier;

        foreach (var axle in _carAxis)
        {
            if (axle.Riding)
            {
                _carSpeed = Mathf.Lerp(_carSpeed, _speed * _transmissionModifier, Time.fixedDeltaTime*5);
                axle.RightWheel.motorTorque = _carSpeed;
                axle.LeftWheel.motorTorque = _carSpeed;

                VisualizeWheel(axle.RightWheel, axle.RightWheelView);
                VisualizeWheel(axle.LeftWheel, axle.LeftWheelView);
            }           

            if (axle.Steering)
            {
                axle.RightWheel.steerAngle = _steerAngle * _joystick.Horizontal / 3;
                axle.LeftWheel.steerAngle = _steerAngle * _joystick.Horizontal / 3;

                _currentAngle = axle.RightWheel.steerAngle * 2;

                VisualizeWheel(axle.RightWheel, axle.RightWheelView);
                VisualizeWheel(axle.LeftWheel, axle.LeftWheelView);
            }
        }

    }

    public void Rotate()
    {
        foreach (var axle in _carAxis)
        {

        }
    }

    public void RotateDefault()
    {
        foreach (var axle in _carAxis)
        {
            if (axle.Steering)
            {
                axle.RightWheel.steerAngle = 0;
                axle.LeftWheel.steerAngle = 0;

                _currentAngle = 0;

                VisualizeWheel(axle.RightWheel, axle.RightWheelView);
                VisualizeWheel(axle.LeftWheel, axle.LeftWheelView);
            }
        }
    }

    public void Boost(float value)
    {
        _carSpeed += value;
    }

    private void SteerAssist()
    {
        if (!_isGrounded)
        {
            return;
        }

        if (Mathf.Abs(_carModel.transform.rotation.eulerAngles.y - _previousYRotation) < 10)
        {
            float turnAjust = (_carModel.transform.rotation.eulerAngles.y - _previousYRotation) * _steerAssistValue;
            Quaternion rotationAssist = Quaternion.AngleAxis(turnAjust, Vector3.up);
            _rigidbody.velocity = rotationAssist * _rigidbody.velocity;
        }
        _previousYRotation = _carModel.transform.rotation.eulerAngles.y;
    }

    private void CheckGround()
    {
        _isGrounded = true;
        foreach (var wheel in _wheelColliders)
        {
            if (!wheel.isGrounded)
            {
                _isGrounded = false;
            }
        }
    }

    private void Brake(int value)
    {
        foreach (var axle in _carAxis)
        {
            if (axle.Riding)
            {
                axle.RightWheel.brakeTorque = value;
                axle.LeftWheel.brakeTorque = value;

                VisualizeWheel(axle.RightWheel, axle.RightWheelView);
                VisualizeWheel(axle.LeftWheel, axle.LeftWheelView);
            }
        }

    }

    private void VisualizeWheel(WheelCollider collider, Transform view)
    {
        Vector3 position;
        Quaternion rotation;

        collider.GetWorldPose(out position, out rotation);

        view.position = position;
        view.rotation = rotation;
    }

    private void LoseSpeed()
    {
        if (_jumped == false)
        {
            return;
        }
        _speed = Mathf.Lerp(_speed, 0, Time.fixedDeltaTime / 3);

        if (_carSpeed < 0.5f)
        {

            Brake(100);
            _result.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out JumpChecker jumper))
        {
            //Time.timeScale = 0.7f;
            _uiPanel.ChangeUi();
            _jumped = true;
        }

        if (other.TryGetComponent(out Checkpoint checkpoint))
        {
            checkpoint.Fade();
            _checkpoints++;
            StatisticChanged?.Invoke(Mathf.RoundToInt(_distance), _obstacles, _checkpoints);
        }

        if (other.TryGetComponent(out Finish finish))
        {
            Brake(2500);
            _result.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Wall wall))
        {
            _obstacles++;
            StatisticChanged?.Invoke(Mathf.RoundToInt(_distance), _obstacles, _checkpoints);
            Brake(1000);

            _wall = true;
            _result.SetActive(true);
        }

        if (collision.gameObject.TryGetComponent(out ElectricPost post))
        {
            _obstacles++;
            StatisticChanged?.Invoke(Mathf.RoundToInt(_distance), _obstacles, _checkpoints);
            Brake(1);
            _carSpeed = 0;
        }

        if (collision.gameObject.TryGetComponent(out Obstacle obstacle))
        {
            _obstacles++;
            StatisticChanged?.Invoke(Mathf.RoundToInt(_distance), _obstacles, _checkpoints);
        }

        if (collision.gameObject.TryGetComponent(out Cone cone))
        {
            _cones++;
            StatisticChanged?.Invoke(Mathf.RoundToInt(_distance), _obstacles, _checkpoints);
        }

        if (collision.gameObject.TryGetComponent(out Road road))
        {
            //Brake(10);
        }
    }

    [System.Serializable]
    public class Axle
    {
        public WheelCollider RightWheel;
        public WheelCollider LeftWheel;

        public Transform RightWheelView;
        public Transform LeftWheelView;

        public bool Steering;
        public bool Riding;
    }
}
