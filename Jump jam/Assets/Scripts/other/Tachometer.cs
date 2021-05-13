using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tachometer : MonoBehaviour
{
    [SerializeField] private Text _speed;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transmission _transmission;    
    [SerializeField] private Drive _car;   
    [SerializeField] private Image _alert;
    [SerializeField] private Knob _knob;
    
    private float _currentSpeed;
    [SerializeField] private float _timer = 0;

    private void OnEnable()
    {
        _transmission.SpeedChanged += OnSpeedChanged;
    }

    private void FixedUpdate()
    {
        _currentSpeed = _car.CarSpeed / 50;
        _speed.text = Mathf.RoundToInt(_currentSpeed).ToString();
       if (_car.CarSpeed / _car.MaxSpeed > 0.9 && !_car.Jumped)
        {
            _timer += Time.fixedDeltaTime;
            if (_timer >= 5f)
            {
                _alert.gameObject.SetActive(true);
                _knob.Alert();
            }

            if (_timer >= 10)
            {
                _transmission.Neutral();
            }
        }
        else
        {
            _alert.gameObject.SetActive(false);
            _knob.StopAlert();
            _timer = 0;
        }
    }

    public void OnSpeedChanged(float value, int mod)
    {
        if (value > 0)
        {
            _animator.SetTrigger("Change");
        }
        else
        {
            _animator.SetTrigger("Lose");
        }
    }
}
   