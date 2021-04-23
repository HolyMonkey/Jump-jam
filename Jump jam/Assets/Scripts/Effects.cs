using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    [SerializeField] private GameObject _sparkEffect;
    [SerializeField] private ParticleSystem[] _boostEffect;
    [SerializeField] private Transform[] _brakeEffectPosition;
    [SerializeField] private GameObject _crushSmoke;
    [SerializeField] private GameObject _crushFire;
    [SerializeField] private CarDrive _car;
    [SerializeField] private GasPedal _gas;
    [SerializeField] private GameObject _result;
    [SerializeField] private Transmission _transmission;
    [SerializeField] private GameObject _brakePrefub;

    private bool _trailCreated = false;


    private void Update()
    {
        Brake();
    }

    private void OnEnable()
    {
        _transmission.SpeedChanged += ChangePatricleLifetime;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Obstacle obstacle))
        {
            //Instantiate(_sparkEffect, collision.contacts[Random.Range(0, collision.contactCount)].point, transform.rotation);
        }

        if (collision.gameObject.TryGetComponent(out Wall wall) || collision.gameObject.TryGetComponent(out ElectricPost post))
        {
            Instantiate(_sparkEffect, collision.contacts[Random.Range(0, collision.contactCount)].point, transform.rotation);
            StartCoroutine(StartSmoke());
        }

        if (collision.gameObject.TryGetComponent(out Road road))
        {
            Instantiate(_sparkEffect, collision.contacts[Random.Range(0, collision.contactCount)].point, transform.rotation);
            StartCoroutine(StartFire());
        }
    }

    private IEnumerator StartSmoke()
    {
        yield return new WaitForSeconds(1);
        _crushSmoke.SetActive(true);
        _result.SetActive(true);
        yield break;
    }

    private IEnumerator StartFire()
    {
        yield return new WaitForSeconds(4);
        if (!_car.IsGrounded)
        {
            _result.SetActive(true);
            _crushFire.SetActive(true);
        }
        yield break;
    }

    public void ChangePatricleLifetime(int mod)
    {
        foreach (var particle in _boostEffect)
        {
            particle.gameObject.SetActive(true);
            particle.startLifetime += mod * 0.035f;
        }
    }

    public void Clearboost()
    {
        foreach (var particle in _boostEffect)
        {
            particle.startLifetime = Mathf.Lerp(particle.startLifetime, 0, Time.fixedDeltaTime * 3);
        }
    }

    public void Brake()
    {
        float timer = 0;

        if (!_car.Jumped)
        {
            return;
        }

        if (_car.IsGrounded && _trailCreated == false && (_car.CurrentAngle < -29 || _car.CurrentAngle > 29))
        {
            timer += Time.fixedDeltaTime;
            Debug.Log("!!!");

            for (int i = 0; i < _brakeEffectPosition.Length; i++)
            {
                Instantiate(_brakePrefub, _brakeEffectPosition[i].position, transform.rotation, transform);
            }
            _trailCreated = true;
        }
    }
}

