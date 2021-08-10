using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JumpJam
{
    public class MonsterTruckTrigger : MonoBehaviour
    {
        [SerializeField] private MonsterTruck _monsterTruck;

        private bool _isColliding;

        public event UnityAction<bool> TriggerChanged;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<EnvironmentObject>(out EnvironmentObject environmentObject))
            {
                if (environmentObject.Level > _monsterTruck.CurrentSize)
                {
                    environmentObject.TurnOnTriggerCollider();
                    _isColliding = true;
                    TriggerChanged?.Invoke(_isColliding);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<EnvironmentObject>(out EnvironmentObject environmentObject))
            {
                if (environmentObject.CheckTriggerIsOn())
                {
                    environmentObject.TurnOffTriggerCollider();
                    _isColliding = false;
                    TriggerChanged?.Invoke(_isColliding);
                }
            }
        }
    }
}
