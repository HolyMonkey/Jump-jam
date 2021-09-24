using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JumpJam
{
    [RequireComponent(typeof(Collider))]
    public class MonsterTruckTrigger : MonoBehaviour
    {
        [SerializeField] private MonsterTruck _monsterTruck;

        private Collider _collider;
        private bool _isColliding;

        public event UnityAction<bool> TriggerChanged;

        private void Start()
        {
            _collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<EnvironmentObject>(out EnvironmentObject environmentObject))
            {
                if (environmentObject.Level > _monsterTruck.CurrentSize)
                {
                    environmentObject.TurnOnTriggerCollider();
                    environmentObject.AddCollider(_collider);
                    _isColliding = true;
                    TriggerChanged?.Invoke(_isColliding);
                }
                else
                {
                    environmentObject.TurnOffTriggerCollider();
                    _isColliding = false;
                    TriggerChanged?.Invoke(_isColliding);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<EnvironmentObject>(out EnvironmentObject environmentObject))
            {
                environmentObject.RemoveCollider(_collider);

                if (environmentObject.CheckTriggerIsOn() && environmentObject.GetCollidersCount() == 0)
                {
                    environmentObject.TurnOffTriggerCollider();
                    _isColliding = false;
                    TriggerChanged?.Invoke(_isColliding);
                }
            }
        }
    }
}
