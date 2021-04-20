using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrackingCamera : MonoBehaviour
{
    [SerializeField] private Transform _anchor;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    private Quaternion _startRotation;
    private Vector3 _offset;


    private void Start()
    {
        _offset = transform.position - _anchor.position;
        _startRotation = transform.rotation;
        //_startRotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, 1);
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _anchor.position + _offset, _moveSpeed * Time.fixedDeltaTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, _anchor.transform.rotation * _startRotation, _rotationSpeed * Time.fixedDeltaTime);        
        // transform.rotation = Quaternion.Lerp(new Quaternion(transform.rotation.x,transform.rotation.y,0,1), _anchor.rotation * _startRotation, _rotationSpeed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(6, transform.rotation.y, 0);
    }
}
