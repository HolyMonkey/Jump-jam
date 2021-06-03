using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TachometerView : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Text _speed;
    [SerializeField] private Text _transmission;
    [SerializeField] private Drive _car;
    [SerializeField] private GasPedal _pedal;

    private void Update()
    {
        _speed.text = Mathf.RoundToInt(_car.CarSpeed / 100).ToString();
        _transmission.text = _pedal.Transmission.ToString();
    }

    public void ChangeSpeed()
    {
        _animator.SetTrigger("TransmissionChanged");
    }

    public void Acceleration()
    {
        _animator.SetTrigger("Acceleration");
    }
}
