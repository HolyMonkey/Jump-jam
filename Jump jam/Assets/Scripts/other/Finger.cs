using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Finger : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private bool _click = false;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _animator.SetTrigger("UpTrigger");
            _animator.SetBool("Click", false);
        }

        if (Input.GetMouseButton(0))
        {

            _animator.SetTrigger("ClickTrigger");
            _animator.SetBool("Click", true);
        }
    }

    private void FixedUpdate()
    {



        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 4)) + new Vector3(0.12f, -0.18f, 0);
    }
}
