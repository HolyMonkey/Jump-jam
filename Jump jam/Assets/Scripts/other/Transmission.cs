using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class Transmission : MonoBehaviour
{
    [SerializeField] private Image _knob;
    [SerializeField] private Transform[] _center;
    [SerializeField] private Transform[] _speed;
    [SerializeField] private Tachometer _tachometer;
    [SerializeField] private Image _image;    

    private int _currentSpeed = 0;
    private Vector3[] path;
    private int _nextSpeed;

    public event UnityAction<float, int> SpeedChanged;

    public int CurrentSpeed => _currentSpeed;

    public void NextSpeed(int value)
    {
        _nextSpeed = value;
    }

    public void ChangeSpeed()
    {
        Debug.Log($"Осуществляю переход с {_currentSpeed} скорости на {_nextSpeed} скорость.");
        if (_currentSpeed > _nextSpeed && _nextSpeed !=0)
        {
            DecreaseSpeed();
        }
        else if (_currentSpeed < _nextSpeed)
        {
            IncreaseSpeed();
        }
        else if (_nextSpeed == 0)
        {
            Neutral();
        }
    }

    public void IncreaseSpeed()
    {
        _knob.gameObject.SetActive(true);

        switch (_currentSpeed)
        {
            case 0:
                path = new Vector3[2] { _center[0].position, _speed[0].position };
                _knob.transform.DOPath(path, 0.5f);
                break;
            case 1:
                path = new Vector3[2] { _center[0].position, _speed[1].position };
                _knob.transform.DOPath(path, 0.5f);
                break;
            case 2:
                path = new Vector3[3] { _center[0].position, _center[1].position, _speed[2].position };
                _knob.transform.DOPath(path, 0.7f);
                break;
            case 3:
                path = new Vector3[2] { _center[1].position, _speed[3].position };
                _knob.transform.DOPath(path, 0.5f);
                break;
            case 4:
                path = new Vector3[3] { _center[1].position, _center[2].position, _speed[4].position };
                _knob.transform.DOPath(path, 0.7f);
                break;
        }
        _currentSpeed++;
        SpeedChanged?.Invoke(1,1);
    }

    public void DecreaseSpeed()
    {
        switch (_currentSpeed)
        {
            case 5:
                path = new Vector3[3] { _center[2].position, _center[1].position, _speed[3].position };
                _knob.transform.DOPath(path, 0.7f);
                break;
            case 4:
                path = new Vector3[2] { _center[1].position, _speed[2].position };
                _knob.transform.DOPath(path, 0.5f);
                break;
            case 3:
                path = new Vector3[3] { _center[1].position, _center[0].position, _speed[1].position };
                _knob.transform.DOPath(path, 0.7f);
                break;
            case 2:
                path = new Vector3[2] { _center[0].position, _speed[0].position };
                _knob.transform.DOPath(path, 0.5f);
                break;
            case 1:
                path = new Vector3[1] { _center[0].position };
                _knob.transform.DOPath(path, 0.25f);
                break;
        }
        _currentSpeed--;
        SpeedChanged?.Invoke(-0.1f,-1);
    }

    public void Neutral()
    {
        switch (_currentSpeed)
        {
            case 5:
                path = new Vector3[1] { _center[2].position };
                _knob.transform.DOPath(path, 0.25f);
                break;
            case 4:
                path = new Vector3[1] { _center[1].position };
                _knob.transform.DOPath(path, 0.25f);
                break;
            case 3:
                path = new Vector3[1] { _center[1].position };
                _knob.transform.DOPath(path, 0.25f);
                break;
            case 2:
                path = new Vector3[1] { _center[0].position };
                _knob.transform.DOPath(path, 0.5f);
                break;
            case 1:
                path = new Vector3[1] { _center[0].position };
                _knob.transform.DOPath(path, 0.7f);
                break;
        }
        _currentSpeed = 0;
        SpeedChanged?.Invoke(-0.1f,0);
    }
}
