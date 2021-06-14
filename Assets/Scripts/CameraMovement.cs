using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    [ExecuteAlways]
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform _target = null;
        [SerializeField] private Vector3 _offset = Vector3.zero;

        private void Update()
        {
            transform.position = _target.position + _offset;
        }
    }
}
