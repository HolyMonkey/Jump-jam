using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarObstacle : MonoBehaviour
{
    [SerializeField] private GameObject _effect;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject[] _effects;

    private bool _crushed = false;


    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Wheel wheel) && !_crushed)
        {
            _crushed = true;
            Instantiate(_effect, _spawnPoint.position, _effect.transform.rotation);
            /*
            int chance = Random.Range(0, 10);
            if(chance < 4)
            {
                Instantiate(_effects[Random.Range(0, 2)], _spawnPoint.position + new Vector3(0, 0, 5), _effect.transform.rotation);
            }     */       
        }
    }
}
