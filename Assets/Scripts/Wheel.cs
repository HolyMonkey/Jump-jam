using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class Wheel : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rootBody = null;
        [SerializeField] private MonsterTruck _monsterTruck;

        public MonsterTruck Truck => _monsterTruck;

        private void Update()
        {
            var temp = new Vector2(-_rootBody.velocity.x, _rootBody.velocity.z);
            transform.Rotate(Vector3.right * (temp.magnitude / 2), Space.Self);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<Wheel>(out Wheel wheel))
            {
                MonsterTruck other = wheel.Truck;
                _monsterTruck.CheckCollision(other);
            }
        }
    }
}
