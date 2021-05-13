using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class GasPedal : MonoBehaviour//, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Slider _slider;    
    [SerializeField] private Drive _car;
    [SerializeField] private Light _leftLight;
    [SerializeField] private Light _rightLight;
    [SerializeField] private GameObject _flare;
    [SerializeField] private GameObject _transmissionBoostEffect;
    [SerializeField] private GameObject _smokeEffects;
    [SerializeField] private MessagePanel _message;
    [SerializeField] private Image _frame;

    private WaitForSeconds _delay = new WaitForSeconds(0.02f);
    private bool _gas = false;
    private float _boostValue;
    private float _previousSliderValue;
    

    public bool Gas => _gas;
    public float Transmission = 0;
    public float TransmissionTimer = 3;

    private void Update()
    {
        Flare();
        CalculateBoost();
        Lights();
    }
    /*
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
    */
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
        if(TransmissionTimer >=2.4 && !_car.Jumped && Transmission < 5)
        {
            _frame.DOColor(new Color(_frame.color.r, _frame.color.g, _frame.color.b, 0.7f), 0.1f);
        }

        TransmissionTimer += Time.deltaTime;
        if(Input.GetMouseButtonDown(0) && Transmission < 5 && TransmissionTimer >= 2.5f && !_car.Jumped)
        {
            TransmissionTimer = 0;
            _frame.DOColor(new Color(_frame.color.r, _frame.color.g, _frame.color.b, 0f), 0.1f);
            Transmission++;
            StartCoroutine(_message.ShowMessage(_message.Messages[Random.Range(0, _message.Messages.Length)]));
            _car.Rigidbody.AddForce((_car.transform.forward * 25000), ForceMode.Impulse);
            _transmissionBoostEffect.SetActive(true);
            _smokeEffects.SetActive(false);
        }

        if(TransmissionTimer >1.0f)
        {
            _smokeEffects.SetActive(true);
            _transmissionBoostEffect.SetActive(false);
        }

        if(TransmissionTimer > 5)
        {
            Transmission--;
            TransmissionTimer = 2;
        }
        
        
        /*if (_slider.value >= 0 && _slider.value <= 5)
        {
            Transmission = _slider.value;
        }
        else
        {
            Transmission = 10 - _slider.value;
        }*/
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
