using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    [SerializeField] private GameObject _sparkEffect;
    [SerializeField] private GameObject _boostEffect;
    [SerializeField] private GameObject _crushSmoke;
    [SerializeField] private GameObject _crushFire;
    [SerializeField] private CarDrive _car;
    [SerializeField] private GasPedal _gas;    

    private void Update()
    {
        if (_gas.Gas)
        {
            _boostEffect.SetActive(true);
        }
        else
        {
            _boostEffect.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Obstacle obstacle))
        {
            Instantiate(_sparkEffect, collision.contacts[Random.Range(0, collision.contactCount)].point, transform.rotation);
        }

        else if (collision.gameObject.TryGetComponent(out Wall wall))
        {
            Instantiate(_sparkEffect, collision.contacts[Random.Range(0, collision.contactCount)].point, transform.rotation);
            StartCoroutine(StartSmoke());
        }

        else if (collision.gameObject.TryGetComponent(out Road road))
        {
            Instantiate(_sparkEffect, collision.contacts[Random.Range(0, collision.contactCount)].point, transform.rotation);
            StartCoroutine(StartFire());
        }
    }

    private IEnumerator StartSmoke()
    {
        yield return new WaitForSeconds(1);
        _crushSmoke.SetActive(true);
        yield break;
    }

    private IEnumerator StartFire()
    {
        yield return new WaitForSeconds(3);
        if (!_car.IsGrounded)
        {
            _crushFire.SetActive(true);
        }
        yield break;
    }
}
