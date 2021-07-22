using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class CarCollisionChecker : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            MonsterTruck monsterTruck = GetComponentInParent<MonsterTruck>();

            if (monsterTruck != null || TryGetComponent<Wheel>(out Wheel wheel))
            {
                Destroy(gameObject);
            }
        }
    }
}
