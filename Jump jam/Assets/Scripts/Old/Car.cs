using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Car : MonoBehaviour
{
    [SerializeField] private float _baseSpeed;
    [SerializeField] private float _currentSpeed;
    [SerializeField] private GasPedal _gas;
    [SerializeField] private GameObject _spark;
    [SerializeField] private bool _fall = false;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private GameObject _brakeEffect;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private UiPanel _uiPanel;
    [SerializeField] private TrackingCamera _camera;

    private WaitForSeconds _delay = new WaitForSeconds(0.1f);
    private bool _jumped = false;
    private bool _land = false;
    [SerializeField] private int _coins = 0;

    

    public event UnityAction<int> CoinTaken;

    public Rigidbody Rigidbody => _rigidbody;
    public bool Jumped => _jumped;
    public bool Fall => _fall;

    public float CurrentSpeed => _currentSpeed;



    private void OnGasDown()
    {
        _rigidbody.velocity = transform.forward * _currentSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out JumpChecker checker))
        {
            Time.timeScale = 0.5f;
            _uiPanel.ChangeUi();
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out JumpChecker checker))
        {
            _jumped = true;
        }

        if (other.TryGetComponent(out Finish fin))
        {
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Obstacle obs) || collision.gameObject.TryGetComponent(out Road rod))
        {
            Time.timeScale = 1f;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Obstacle obstacle))
        {
            var chance = Random.Range(0, 10);
            if (chance == 0)
            {
                Instantiate(_spark, collision.contacts[Random.Range(0, collision.contactCount)].point, transform.rotation);
            }
        }

        if (collision.gameObject.TryGetComponent(out Road road) && _currentSpeed > 0.03f && _jumped && _fall)
        {
            var chance = Random.Range(0, 10);
            if (chance == 0)
            {
                Instantiate(_spark, collision.contacts[Random.Range(0, collision.contactCount)].point, transform.rotation);
            }
        }        
    }

    private void CheckAngle()
    {
        if (transform.rotation.z > 40 * Mathf.Deg2Rad || transform.rotation.z < -40 * Mathf.Deg2Rad)
        {
            _fall = true;
        }
    }

    public void GetCoin()
    {        
        _coins++;
        CoinTaken?.Invoke(_coins);
    }

    private IEnumerator LoseSpeed(float value)
    {
        while (_baseSpeed > 0)
        {
            _baseSpeed -= Time.deltaTime * value;

            if (_baseSpeed < 0)
            {
                _baseSpeed = 0;
            }

            yield return _delay;
        }
        yield break;
    }
}
