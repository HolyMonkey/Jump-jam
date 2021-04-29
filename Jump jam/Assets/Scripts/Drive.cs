using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{
    [SerializeField] private GameObject _carBody;
    [SerializeField] private Axle[] _carAxis;
    [SerializeField] private WheelCollider[] _wheelColliders;
    [SerializeField] private float _carSpeed;
    [SerializeField] private Transform _centerOfMass;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transmission _transmission;
    [SerializeField] private float _transmissionModifier;
    [SerializeField] private float _speed;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private float _currentMaxTransitionModifier;
    [SerializeField] private GasPedal _pedal;
    [Range(0, 1)] [SerializeField] private float _steerAssistValue;


    
    private float _steerAngle = 30;
    private float _previousYRotation;   
    
    public float MaxSpeed;
    public bool Jumped = false;
    public bool IsGrounded;
    public bool Finished = false;
    public int CarSmashed = 0;
    public float CarSpeed => _carSpeed;

    private void Start()
    {
        _rigidbody.centerOfMass = _centerOfMass.localPosition;
    }

    private void FixedUpdate()
    {
        CheckGround();
        Accelerate();
        SteerAssist();
    }

    private void Accelerate()
    {
        if (!Jumped)
        {
            switch (Mathf.RoundToInt(_pedal.Transmission))
            {
                case 0:
                    _transmissionModifier = 0;
                    break;
                case 1:
                    _currentMaxTransitionModifier = 0.5f;
                    _transmissionModifier = Mathf.Lerp(_transmissionModifier, _currentMaxTransitionModifier, Time.fixedDeltaTime);
                    break;
                case 2:
                    _currentMaxTransitionModifier = 1f;
                    _transmissionModifier = Mathf.Lerp(_transmissionModifier, _currentMaxTransitionModifier, Time.fixedDeltaTime);
                    break;
                case 3:
                    _currentMaxTransitionModifier = 1.5f;
                    _transmissionModifier = Mathf.Lerp(_transmissionModifier, _currentMaxTransitionModifier, Time.fixedDeltaTime);
                    break;
                case 4:
                    _currentMaxTransitionModifier = 2f;
                    _transmissionModifier = Mathf.Lerp(_transmissionModifier, _currentMaxTransitionModifier, Time.fixedDeltaTime);
                    break;
                case 5:
                    _currentMaxTransitionModifier = 2.5f;
                    _transmissionModifier = Mathf.Lerp(_transmissionModifier, _currentMaxTransitionModifier, Time.fixedDeltaTime);
                    break;
            }
        }
        else
        {
            _currentMaxTransitionModifier = Mathf.Lerp(_currentMaxTransitionModifier, 0, Time.fixedDeltaTime);
            _transmissionModifier = Mathf.Lerp(_transmissionModifier, _currentMaxTransitionModifier, Time.fixedDeltaTime);
            _rigidbody.drag = 0.2f;
        }

        foreach (var axle in _carAxis)
        {
            if (axle.Riding)
            {
                _carSpeed = _speed * _transmissionModifier; //Mathf.Lerp(_carSpeed, _speed * _transmissionModifier, Time.fixedDeltaTime * 5);
                axle.RightWheel.motorTorque = _carSpeed;
                axle.LeftWheel.motorTorque = _carSpeed;

                VisualizeWheel(axle.RightWheel, axle.RightWheelView);
                VisualizeWheel(axle.LeftWheel, axle.LeftWheelView);
            }

            if (axle.Steering)
            {
                axle.RightWheel.steerAngle = _steerAngle * _joystick.Horizontal / 3f;
                axle.LeftWheel.steerAngle = _steerAngle * _joystick.Horizontal / 3f;

                VisualizeWheel(axle.RightWheel, axle.RightWheelView);
                VisualizeWheel(axle.LeftWheel, axle.LeftWheelView);
            }
        }
        MaxSpeed = _speed * _currentMaxTransitionModifier;               
    }

    private void SteerAssist()
    {
        if (!IsGrounded)
        {
            return;
        }

        if (Mathf.Abs(_carBody.transform.rotation.eulerAngles.y - _previousYRotation) < 10)
        {
            float turnAjust = (_carBody.transform.rotation.eulerAngles.y - _previousYRotation) * _steerAssistValue;
            Quaternion rotationAssist = Quaternion.AngleAxis(turnAjust, Vector3.up);
            _rigidbody.velocity = rotationAssist * _rigidbody.velocity;
        }
        _previousYRotation = _carBody.transform.rotation.eulerAngles.y;
    }

    private void CheckGround()
    {
        IsGrounded = true;
        foreach (var wheel in _wheelColliders)
        {
            if (!wheel.isGrounded)
            {
                IsGrounded = false;
            }
        }
    }

    private void VisualizeWheel(WheelCollider collider, Transform view)
    {
        Vector3 position;
        Quaternion rotation;

        collider.GetWorldPose(out position, out rotation);

        if (!Jumped)
        {
            view.position = new Vector3(position.x, Mathf.Lerp(view.position.y, position.y, Time.fixedDeltaTime), position.z);
        }
        else
        {
            view.position = position;
        }

        view.rotation = rotation;
    }

    public void Brake(int value)
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

    public void Nitro()
    {
        _rigidbody.AddForce(_carBody.transform.forward * 50, ForceMode.Impulse);
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
