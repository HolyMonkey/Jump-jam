using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GasPedal : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Slider _slider;    
    [SerializeField] private CarDrive _car;


    private WaitForSeconds _delay = new WaitForSeconds(0.02f);
    private bool _gas = false;
    private float _boostValue;

    public bool Gas => _gas;


    private void Update()
    {
        CalculateBoost();
    }

    public void OnPointerDown(PointerEventData eventData)
    {        
        _gas = true;
        StartCoroutine(Acceleration());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _gas = false;
        StartCoroutine(Deceleration());
    }

    private IEnumerator Acceleration()
    {
        while (_gas)
        {
            _slider.value++;
            _car.Boost(_boostValue);
            yield return _delay;
        }
        yield break;
    }

    private IEnumerator Deceleration()
    {
        while (!_gas)
        {
            _slider.value--;
            if (_car.CarSpeed > _car.StartSpeed + Mathf.Abs(_boostValue))
            {
                _car.Boost(-1 * Mathf.Abs(_boostValue));
            }
            yield return _delay;
        }
        yield break;
    }

    private void CalculateBoost()
    {
        if (_slider.value >= 0 && _slider.value <= 50)
        {
            _boostValue = _slider.value;
        }
        else
        {
            _boostValue = -1 * (100 - _slider.value);
        }
    }
}
