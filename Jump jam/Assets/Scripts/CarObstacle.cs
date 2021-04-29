using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarObstacle : MonoBehaviour
{
    [SerializeField] GameObject _effect;
    [SerializeField] Transform _spawnPoint;

    private bool _crushed = false;


    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Wheel wheel) && !_crushed)
        {
            _crushed = true;
            Instantiate(_effect, _spawnPoint.position, _effect.transform.rotation);
        }
    }
}
