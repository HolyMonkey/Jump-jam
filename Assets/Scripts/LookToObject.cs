using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class LookToObject : MonoBehaviour
    {
        [SerializeField] private Transform _target = null;

        private void Update()
        {
            transform.LookAt(_target);
        }
    }
}
