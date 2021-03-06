using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GasPedal : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Slider _slider;    
    [SerializeField] private Drive _car;
    [SerializeField] private Light _leftLight;
    [SerializeField] private Light _rightLight;
    [SerializeField] private GameObject _flare;


    private WaitForSeconds _delay = new WaitForSeconds(0.02f);
    private bool _gas = false;
    private float _boostValue;
    private float _previousSliderValue;
    

    public bool Gas => _gas;
    public float Transmission = 0;

    private void Update()
    {
        Flare();
        CalculateBoost();
        Lights();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _previousSliderValue = _slider.value;
        _gas = true;
        StartCoroutine(Acceleration());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _previousSliderValue = _slider.value;
        _gas = false;
        StartCoroutine(Deceleration());
    }

    private IEnumerator Acceleration()
    {
        while (_gas && Transmission <5)
        {           
            _slider.value += Time.fixedDeltaTime * 7;
            yield return _delay;
        }
        yield break;
    }

    private IEnumerator Deceleration()
    {
        while (!_gas && _slider.value > 0 + Time.fixedDeltaTime)
        {            
            _slider.value -= Time.fixedDeltaTime * 7;
            yield return _delay;
        }
        yield break;
    }

    private void CalculateBoost()
    {
        if (_slider.value >= 0 && _slider.value <= 5)
        {
            Transmission = _slider.value;
        }
        else
        {
            Transmission = 10 - _slider.value;
        }
    }

    private void Flare()
    {
        if (_slider.value >= 3 && _slider.value <= 7)
        {
            _flare.SetActive(true);
        }
        else _flare.SetActive(false);
    }

    private void Lights()
    {
        if(_car.Jumped)
        {
            return;
        }

        if (_previousSliderValue - _slider.value > 2f  && _slider.value < 5)
        {
            _leftLight.intensity = Mathf.Lerp(_leftLight.intensity, 4, Time.fixedDeltaTime);
            _rightLight.intensity = Mathf.Lerp(_rightLight.intensity, 4, Time.fixedDeltaTime);
        }
        else
        {
            _leftLight.intensity = Mathf.Lerp(_leftLight.intensity, 0, Time.fixedDeltaTime);
            _rightLight.intensity = Mathf.Lerp(_rightLight.intensity, 0, Time.fixedDeltaTime);
        }        
    }
}
