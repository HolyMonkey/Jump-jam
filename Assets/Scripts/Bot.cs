using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JumpJam
{
    public class Bot : MonoBehaviour, IInputPresenter
    {
        [SerializeField] private float _reachDistance = 0.5f;
        [SerializeField] private List<Vector2> _waypoints = new List<Vector2>();
        private int _currentWayPoint = 0;

        public List<Vector3> Waypoints => _waypoints.Select(x => new Vector3(x.x, 0, x.y)).ToList();

        private void OnDrawGizmosSelected()
        {
            if (_waypoints.Count < 1)
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position + Vector3.up * 2 * (transform.localScale.y / 1.4f), (Waypoints[_currentWayPoint] - transform.position).normalized * 2);
            Gizmos.DrawSphere(transform.position + Vector3.up * 2 * (transform.localScale.y / 1.4f) + (Waypoints[_currentWayPoint] - transform.position).normalized * 2, 0.1f);

            Gizmos.color = Color.white;

            Gizmos.DrawSphere(Waypoints[0] + Vector3.up * 2, 0.5f);
            for (int i = 1; i < _waypoints.Count; i++)
            {
                Gizmos.DrawSphere(Waypoints[i] + Vector3.up * 2, 0.5f);
                Gizmos.DrawLine(Waypoints[i - 1] + Vector3.up * 2, Waypoints[i] + Vector3.up * 2);
            }
        }

        private void Update()
        {
            var position = transform.position;
            position.y = 0;
            float distance = Vector3.Distance(Waypoints[_currentWayPoint], position);

            if (distance <= _reachDistance)
            {
                _currentWayPoint++;

                if (_currentWayPoint >= _waypoints.Count)
                {
                    _currentWayPoint = 0;
                }
            }
        }

        public Vector2 GetCurrentInput()
        {
            var destination = Quaternion.AngleAxis(45, Vector3.up) * (Waypoints[_currentWayPoint] - transform.position).normalized;
            return new Vector2(destination.x, destination.z);
        }
    }
}
