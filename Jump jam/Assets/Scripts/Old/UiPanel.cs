using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class UiPanel : MonoBehaviour
{
    [SerializeField] private Drive _car;
    [SerializeField] private Drive _botCar;
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _x10;
    [SerializeField] private GameObject _x20;
    [SerializeField] private GameObject _x30;
    [SerializeField] private GameObject _nitro;
    [SerializeField] private GameObject _result;
    [SerializeField] private GameObject _amazing;

    private bool _show = false;
    private bool _amazingJump = false;

    private void Update()
    {
        if (_car.CarSmashed == 40)
        {
            StartCoroutine(ShowEffect(_x10));
        }
        if (_car.CarSmashed == 80)
        {
            StartCoroutine(ShowEffect(_x20));
        }
        if (_car.CarSmashed == 120)
        {
            StartCoroutine(ShowEffect(_x30));
        }        
    }

    private void FixedUpdate()
    {        
        if (_botCar.transform.position.z < _car.transform.position.z && _amazingJump != true)
        {
            _amazingJump = true;
            StartCoroutine(ShowAmazing(_amazing));
        }
    }

    public IEnumerator ChangeUi()
    {
        transform.DOMove(_target.position, 0.5f);
        _nitro.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        _nitro.SetActive(false);
        Time.timeScale = 1;
        _car.Nitro();
        yield return new WaitForSeconds(7);
        _result.SetActive(true);
        yield break;
    }

    private IEnumerator ShowEffect(GameObject value)
    {
        if (!_show)
        {
            _show = true;
            value.SetActive(true);
            yield return new WaitForSeconds(2);
            value.SetActive(false);
            _show = false;
            yield break;
        }
    }
    private IEnumerator ShowAmazing(GameObject value)
    {
        value.SetActive(true);
        yield return new WaitForSeconds(2);
        value.SetActive(false);        
        yield break;
    }
}
