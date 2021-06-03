using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UiWheel : MonoBehaviour
{
    [SerializeField] private GameObject _wheel;
    [SerializeField] private int _angle;
    [SerializeField] private CarDrive _car;
    [SerializeField] private Joystick _joystick;    

    private void Update()
    {
        _wheel.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(_wheel.transform.rotation.z, 90 * -_joystick.Horizontal, 0.5f));
    }
}
