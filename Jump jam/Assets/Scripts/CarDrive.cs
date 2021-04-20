using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarDrive : MonoBehaviour
{
    [SerializeField] private GameObject _carModel;
    [SerializeField] private Axle[] _carAxis;
    [SerializeField] private WheelCollider[] _wheelColliders;
    [SerializeField] private float _carSpeed;
    [SerializeField] private Transform _centerOfMass;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private UiPanel _uiPanel;
    [Range(0, 1)] [SerializeField] private float _steerAssistValue;

    private float _steerAngle = 25;
    private float _verticalInput;
    private float _horizontalInput;
    private bool _isGrounded;
    private float _previousYRotation;
    [SerializeField] private bool _jumped = false;
    private float _startSpeed;

    public float StartSpeed => _startSpeed;
    public bool IsGrounded => _isGrounded;
    public float CarSpeed => _carSpeed;

    private void FixedUpdate()
    {
        _verticalInput = Input.GetAxis("Vertical");
        _horizontalInput = Input.GetAxis("Horizontal");

        CheckGround();
        Accelerate();
        LoseSpeed();
        SteerAssist();
    }

    private void Start()
    {
        _startSpeed = _carSpeed;
        _rigidbody.centerOfMass = _centerOfMass.localPosition;
    }

    private void Accelerate()
    {        
        foreach (var axle in _carAxis)
        {
            if (axle.Riding)
            {
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
                axle.RightWheel.steerAngle = _steerAngle * Mathf.Lerp(0, value, Time.fixedDeltaTime * 10);
                axle.LeftWheel.steerAngle = _steerAngle * Mathf.Lerp(0, value, Time.fixedDeltaTime * 10);

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

        _carSpeed = Mathf.Lerp(_carSpeed, 1, Time.fixedDeltaTime / 2f);

        if (_carSpeed < 2)
        {
            Brake(100);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _uiPanel.ChangeUi();
        _jumped = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Wall wall))
        {
            Brake(1000);
        }

        else if (collision.gameObject.TryGetComponent(out Obstacle obstacle))
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
