using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform _target = null;
        [SerializeField] private Vector3 _offset = Vector3.zero;
        [SerializeField] private float _speed = 1;

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, _target.position + _offset, _speed * Time.deltaTime);
        }
    }
}
