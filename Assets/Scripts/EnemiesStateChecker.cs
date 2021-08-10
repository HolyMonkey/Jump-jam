using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JumpJam
{
    public class EnemiesStateChecker : MonoBehaviour
    {
        [SerializeField] private List<MonsterTruck> trucks;

        private int _trucksCount;

        public event UnityAction EnemiesDestroyed;

        private void OnEnable()
        {
            for (int i = 0; i < trucks.Count; i++)
            {
                trucks[i].Destroyed += OnTruckDestroyed;
            }

            _trucksCount = trucks.Count;
        }

        private void OnDisable()
        {
            for (int i = 0; i < trucks.Count; i++)
            {
                trucks[i].Destroyed -= OnTruckDestroyed;
            }
        }

        private void OnTruckDestroyed()
        {
            _trucksCount--;

            if (_trucksCount == 0)
                EnemiesDestroyed?.Invoke();
        }
    }
}
