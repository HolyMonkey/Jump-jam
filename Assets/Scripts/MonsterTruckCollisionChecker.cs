using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class MonsterTruckCollisionChecker : MonoBehaviour
    {
        [SerializeField] private MonsterTruck _truck;
        public MonsterTruck Truck => _truck;
    }
}
