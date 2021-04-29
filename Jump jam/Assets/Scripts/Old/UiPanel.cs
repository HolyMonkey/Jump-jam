using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class UiPanel : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _nitro;

    public void ChangeUi()
    {
        transform.DOMove(_target.position, 0.5f);
        _nitro.SetActive(true);
    }


}
