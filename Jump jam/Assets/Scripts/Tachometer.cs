using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tachometer : MonoBehaviour
{
    [SerializeField] private Text _speed;
    [SerializeField] private Text _transmissionValue;
    [SerializeField] private Transmission transmission;
    [SerializeField] private GameObject _needle;
    [SerializeField] private CarDrive _car;

    private Vector3 _currentNeedleRotation = new Vector3(0, 0, 178);

    private void Start()
    {
        _needle.transform.eulerAngles = _currentNeedleRotation;
    }

    private void Update()
    {
        _speed.text = Mathf.RoundToInt(_car.CarSpeed / 10).ToString();
        _transmissionValue.text = transmission.CurrentSpeed.ToString();
        _needle.transform.rotation = Quaternion.Euler(0, 0, -_currentNeedleRotation.z - _car.CarSpeed/5);
    }
}
