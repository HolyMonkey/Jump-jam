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
        _offset = transform.position - _anchor.position; //new Vector3(0, 5f, -10);
        _startRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _anchor.position + _offset, _moveSpeed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, _anchor.rotation * _startRotation, _rotationSpeed * Time.fixedDeltaTime);
    }

    public void StopRotate()
    {
        _rotationSpeed = 0.01f;
    }
}
