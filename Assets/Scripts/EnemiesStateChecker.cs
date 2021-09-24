using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JumpJam
{
    public class EnemiesStateChecker : MonoBehaviour
    {
        [SerializeField] private List<MonsterTruck> _trucks;

        public event UnityAction EnemiesDestroyed;

        private void OnEnable()
        {
            for (int i = 0; i < _trucks.Count; i++)
            {
                _trucks[i].Destroyed += OnTruckDestroyed;
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _trucks.Count; i++)
            {
                _trucks[i].Destroyed -= OnTruckDestroyed;
            }
        }

        private void OnTruckDestroyed(MonsterTruck truck)
        {
            _trucks.Remove(truck);

            if (_trucks.Count == 0)
                EnemiesDestroyed?.Invoke();
        }
    }
}
