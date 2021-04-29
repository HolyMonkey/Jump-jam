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

    private bool _rotate = false;

    private void Update()
    {
        _wheel.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(_wheel.transform.rotation.z, 90 * -_joystick.Horizontal, 0.5f));
    }

    /*
    public IEnumerator Rotate(int angle)
    {
        _wheel.transform.DORotate(new Vector3(0, 0, 45 * -angle), 0.5f);
        while (_rotate)
        {
            _car.Rotate(_angle * 60);
            yield return new WaitForSeconds(0.2f);
        }
        yield break;
    }*/
}
