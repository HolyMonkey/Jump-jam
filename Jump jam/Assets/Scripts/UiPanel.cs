using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class UiPanel : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _wheel;

    public void ChangeUi()
    {
        transform.DOMove(_target.position, 1f);
        _wheel.SetActive(true);
        _wheel.transform.DOScale(new Vector3(4, 4, 1), 1);
    }

    public IEnumerator Hide()
    {
        _wheel.transform.DOScale(new Vector3(0.01f, 0.01f, 1), 0.3f);
        //_wheel.transform.DOMove(_target.position, 1f);
        yield return new WaitForSeconds(0.3f);
        _wheel.gameObject.SetActive(false);
    }
}
