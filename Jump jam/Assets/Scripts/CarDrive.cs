using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarDrive : MonoBehaviour
{
    [SerializeField] private GameObject _carModel;
    [SerializeField] private Axle[] _carAxis;
    [SerializeField] private WheelCollider[] _wheelColliders;
    private float _carSpeed;
    [SerializeField] private Transform _centerOfMass;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private UiPanel _uiPanel;
    [SerializeField] private GameObject _result;
    [Range(0, 1)] [SerializeField] private float _steerAssistValue;

    [SerializeField] private Effects _effects;
    [SerializeField] private Transmission _transmission;
    private float _transmissionModifier;
    private float _speed = 300;
    private float _distance = 32;
    private float _steerAngle = 25;
    private bool _isGrounded;
    private float _previousYRotation;
    private bool _jumped = false;
    private float _startSpeed;
    [SerializeField] private float _currentAngle;

    [SerializeField] private int _cones = 0;
    [SerializeField] private int _checkpoints = 0;
    [SerializeField] private int _obstacles = 0;
    [SerializeField] private bool _wall = false;

    public event UnityAction<int, int, int> StatisticChanged;
        

    public float CurrentAngle => _currentAngle;
    public int Cones => _cones;
    public int Checkpoints => _checkpoints;
    public int Obstacles => _obstacles;
    public bool Wall => _wall;

    public float StartSpeed => _startSpeed;
    public bool IsGrounded => _isGrounded;
    public float CarSpeed => _carSpeed;
    public bool Jumped => _jumped;

    public float Distance => _distance;

    private void FixedUpdate()
    {
        if (_transmission.CurrentSpeed > 0)
        {
            _distance =32 + transform.position.z;
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

    private void Accelerate()
    {
        if (!Jumped)
        {
            switch (_transmission.CurrentSpeed)
            {
                case 0:
                    _transmissionModifier = 0;
                    break;
                case 1:
                    _transmissionModifier = Mathf.Lerp(_transmissionModifier, 1, Time.fixedDeltaTime);                    
                    break;                    
                case 2:
                    _transmissionModifier = Mathf.Lerp(_transmissionModifier, _transmissionModifier * 1.2f, Time.fixedDeltaTime*1.5f);
                    break;
                case 3:
                    _transmissionModifier = Mathf.Lerp(_transmissionModifier, _transmissionModifier * 1.2f, Time.fixedDeltaTime*1.5f);
                    break;
                case 4:
                    _transmissionModifier = Mathf.Lerp(_transmissionModifier, _transmissionModifier * 1.2f, Time.fixedDeltaTime*1.5f);
                    break;
                case 5:
                    _transmissionModifier = Mathf.Lerp(_transmissionModifier, _transmissionModifier * 1.2f, Time.fixedDeltaTime*1.5f);
                    break;
            }
        }
        else
        {
            LoseSpeed();
            _effects.Clearboost();
        }

        foreach (var axle in _carAxis)
        {
            if (axle.Riding)
            {
                _carSpeed = _speed * _transmissionModifier;
                axle.RightWheel.motorTorque = _carSpeed; // _verticalInput;
                axle.LeftWheel.motorTorque = _carSpeed; // _verticalInput;

                //Debug.Log(axle.RightWheel.motorTorque + " " + axle.LeftWheel.motorTorque + " " + _carSpeed);

                VisualizeWheel(axle.RightWheel, axle.RightWheelView);
                VisualizeWheel(axle.LeftWheel, axle.LeftWheelView);
            }
        }

    }

    public void Rotate(int value)
    {
        foreach (var axle in _carAxis)
        {
            if (axle.Steering)
            {
                axle.RightWheel.steerAngle = _steerAngle * Mathf.Lerp(0, value, Time.fixedDeltaTime);
                axle.LeftWheel.steerAngle = _steerAngle * Mathf.Lerp(0, value, Time.fixedDeltaTime);

                _currentAngle = axle.RightWheel.steerAngle;

                VisualizeWheel(axle.RightWheel, axle.RightWheelView);
                VisualizeWheel(axle.LeftWheel, axle.LeftWheelView);
            }
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
        _uiPanel.Hide();
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
            StartCoroutine(_uiPanel.Hide());
            Brake(100);
            _result.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out JumpChecker jumper))
        {
            Time.timeScale = 0.7f;
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
            Debug.Log("FINISH");
            Brake(1000);
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
            StartCoroutine(_uiPanel.Hide());
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
            Time.timeScale = 1;
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
