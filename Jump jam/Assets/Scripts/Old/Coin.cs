using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private void Update()
    {
        if(transform.position == _target.transform.position)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Car car))
        {
            transform.DOMove(_target.position, 20f);
            //transform.DOScale(new Vector3(0.1f,0.1f,0.1f), 1);
            car.GetCoin();
        }
    }
}
