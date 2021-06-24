using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    public class CameraDepthBuffer : MonoBehaviour
    {
        private Camera _camera = null;

        private void OnEnable()
        {
            _camera = GetComponent<Camera>();

            _camera.depthTextureMode = DepthTextureMode.Depth;
        }

        private void OnValidate()
        {
            _camera = GetComponent<Camera>();

            _camera.depthTextureMode = DepthTextureMode.Depth;
        }
    }
}
