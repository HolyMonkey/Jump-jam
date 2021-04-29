using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Finger : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private bool _click = false;

    private void FixedUpdate()
    {
        //transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5)) + new Vector3(0.12f,-0.18f,0);
        transform.position = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x*1024, Input.mousePosition.y*1280, -1));

        if (Input.GetMouseButtonDown(0) && !_click)
        {
            _click = true;
            StartCoroutine(Click());
        }        


    }

    private IEnumerator Click()
    {
        transform.eulerAngles = transform.eulerAngles + new Vector3(30, 0, 0);
        yield return new WaitForSeconds(0.1f);
        transform.eulerAngles = transform.eulerAngles - new Vector3(30, 0, 0);
        _click = false;
        yield break;
    }
}
