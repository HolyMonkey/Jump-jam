using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class UiPanel : MonoBehaviour
{
    [SerializeField] private Drive _car;
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _nitro;
    [SerializeField] private GameObject _result;


    public IEnumerator ChangeUi()
    {
        transform.DOMove(_target.position, 0.5f);
        _nitro.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        _nitro.SetActive(false);
        Time.timeScale = 1;
        _car.JumpNitro();
        yield return new WaitForSeconds(7);
        _result.SetActive(true);
        yield break;
    }
}
