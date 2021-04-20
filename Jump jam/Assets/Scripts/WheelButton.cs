using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class WheelButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private UiWheel _wheel;
    [SerializeField] private  int _angle;
    [SerializeField] private CarDrive _car;

    private bool _rotate = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        _rotate = true;
        StartCoroutine(Rotate(_angle));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _rotate = false;
        _car.RotateDefault();
        _wheel.transform.DORotate(new Vector3(0, 0, 0), 0.5f);
    }

    public IEnumerator Rotate(int angle)
    {
        while(_rotate)
        {
            _wheel.transform.Rotate(new Vector3(0, 0, -angle * 5));
            _car.Rotate(_angle * 20);
            yield return new WaitForSeconds(0.1f);            
        }
        yield break;
    }
}
