using System.Collections.Generic;
using UnityEngine;
using EasyRoads3Dv3;
using System.Linq;
using System;
using JumpJam.Extensions;

namespace JumpJam.TrafficSystem
{
    public class AutoMgmt : MonoBehaviour
    {
        private const float REACH_DISTANCE = 0.005f;

        [SerializeField] private float _speed = 40f;
        [SerializeField] private float _acceleration = 10f;
        [SerializeField] private float _rotationSpeed = 0.5f;
        [SerializeField] private Vector3 _rayOffset = Vector3.zero;
        [SerializeField] private float _rayDistance = 5.0f;

        private int _currentWayPoint = 0;
        private List<Vector3> _waypoints = new List<Vector3>();
        private bool _startAtStart = false;
        private float _currentVelocity = 0;

        [field: SerializeField] public Transform HoverAuto { get; set; }
        [field: SerializeField] public float MinRight { get; set; }
        [field: SerializeField] public float MaxRight { get; set; }
        public ERRoad CurrentRoad { get; set; }
        public ERRoad PrevRoad { get; set; }
        //public int CurrentWayPoint { get; set; }

        private void Start()
        {
            if (CurrentRoad == null)
            {
                Debug.Log("Error: AutoMgmt is set active before a ERRoad object has been added.");
            }
            else
            {
                var centerPoints = CurrentRoad.GetSplinePointsCenter();
                var distanceStart = Vector3.Distance(centerPoints[0], transform.position);
                var distanceEnd = Vector3.Distance(centerPoints[centerPoints.Length - 1], transform.position);

                if (distanceStart > distanceEnd)
                {
                    Array.Reverse(centerPoints);
                    _startAtStart = true;
                }
                else
                {
                    _startAtStart = false;
                }

                var minDistance = float.MaxValue;
                var minIndex = 0;
                for (int i = 1, j = 0; i < centerPoints.Length; i += 2, j++)
                {
                    var distance = Vector3.Distance(centerPoints[i], transform.position);
                    var nextDistance = Vector3.Distance(centerPoints[i], transform.position + transform.forward * 1);
                    if (distance < minDistance || nextDistance < minDistance)
                    {
                        minDistance = distance;
                        minIndex = j;
                    }

                    _waypoints.Add(centerPoints[i]);
                }

                //if (CurrentWayPoint >= _waypoints.Count)
                //{
                //    CurrentWayPoint -= 1;
                //}
                _currentWayPoint = minIndex;

                //GetNextPath();
            }
        }

        /*/
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
        //*/

        //*/
        private void OnDrawGizmosSelected()
        {
            var leftPoints = CurrentRoad.GetSplinePointsLeftSide().AsEnumerable();
            var centerPoints = CurrentRoad.GetSplinePointsCenter().AsEnumerable();
            var rightPoints = CurrentRoad.GetSplinePointsRightSide().AsEnumerable();

            if (PrevRoad != null)
            {
                leftPoints = leftPoints.Concat(PrevRoad.GetSplinePointsLeftSide());
                centerPoints = centerPoints.Concat(PrevRoad.GetSplinePointsCenter());
                rightPoints = rightPoints.Concat(PrevRoad.GetSplinePointsRightSide());
            }

            Gizmos.color = Color.red;
            foreach (var point in leftPoints)
            {
                Gizmos.DrawSphere(point, 1);
            }

            Gizmos.color = Color.green;
            foreach (var point in centerPoints)
            {
                Gizmos.DrawSphere(point, 1);
            }

            Gizmos.color = Color.blue;
            foreach (var point in rightPoints)
            {
                Gizmos.DrawSphere(point, 1);
            }

            Gizmos.color = Color.white;
            foreach (var point in _waypoints)
            {
                Gizmos.DrawSphere(point + Vector3.up * 2, 1);
            }
        }
        //*/

        private void FixedUpdate()
        {
            float distance = Vector3.Distance(_waypoints[_currentWayPoint], transform.position);

            if (distance <= REACH_DISTANCE)
            {
                _currentWayPoint++;

                if (_currentWayPoint >= _waypoints.Count)
                {
                    GetNextPath();
                }
            }

            if (_currentWayPoint <= 16)
            {
                _currentVelocity = Mathf.Lerp(_currentVelocity, _speed - 5, _acceleration / 3 * Time.deltaTime);

            }
            else if (_currentWayPoint <= 20)
            {
                _currentVelocity = Mathf.Lerp(_currentVelocity, _speed, _acceleration / 3 * Time.deltaTime);

            }
            else
            {
                _currentVelocity = Mathf.Lerp(_currentVelocity, _speed, _acceleration * Time.deltaTime);
            }

            transform.position = Vector3.MoveTowards(transform.position, _waypoints[_currentWayPoint], _currentVelocity * Time.deltaTime);

            var toTarget = _waypoints[_currentWayPoint] - transform.position;
            if (toTarget != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(toTarget);
                var t = _currentVelocity.Remap(0, _speed, 0, 1);
                var val1 = Quaternion.RotateTowards(transform.rotation, targetRotation, Mathf.Lerp(0, _rotationSpeed * Time.deltaTime, t));
                var val2 = Quaternion.Slerp(transform.rotation, targetRotation, Mathf.Lerp(0, _rotationSpeed / 10 * Time.deltaTime, t));
                transform.rotation = Quaternion.Angle(val1, val2) < _rotationSpeed * Time.deltaTime ? val2 : val1;
            }
        }

        private void GetNextPath()
        {
            ERConnection erConnection;

            if (_startAtStart)
            {
                erConnection = CurrentRoad.GetConnectionAtStart();
            }
            else
            {
                erConnection = CurrentRoad.GetConnectionAtEnd();
            }

            //This happens if the road is a dead-end (and??)
            if (erConnection == null)
            {
                if (_startAtStart)
                {
                    erConnection = CurrentRoad.GetConnectionAtEnd();
                }
                else
                {
                    erConnection = CurrentRoad.GetConnectionAtStart();
                }
            }

            List<ERConnectionData> erConnectionData = erConnection.GetConnectionData().ToList();

            //If there is more than one connection, delete the current connection from the list (don't want to go back the way we came)
            if (erConnectionData.Count > 1)
            {
                for (int i = 0; i < erConnectionData.Count; i++)
                {
                    if (erConnectionData[i].road == CurrentRoad || erConnectionData[i].road == PrevRoad)
                    {
                        erConnectionData.RemoveAt(i);
                    }
                }
            }

            //Get the next road
            int iNext = UnityEngine.Random.Range(0, erConnectionData.Count);
            PrevRoad = CurrentRoad;
            CurrentRoad = erConnectionData[iNext].road;

            Vector3[] centerPoints = CurrentRoad.GetSplinePointsCenter();
            float distanceStart = Vector3.Distance(centerPoints[0], transform.position);
            float distanceEnd = Vector3.Distance(centerPoints[centerPoints.Length - 1], transform.position);

            if (distanceStart > distanceEnd)
            {
                Array.Reverse(centerPoints);
                _startAtStart = true;
            }
            else
            {
                _startAtStart = false;
            }

            //Add a marker part way into the intersection to add as a way-point between the two roads
            //This is so the autos don't cut across the curb and/or into buildings
            if (_waypoints.Count > 0)
            {
                //var intersectionPoint = IntersectionVector(erConnection.gameObject.transform.position, _waypoints[_waypoints.Count - 1]);

                var from = _waypoints[_waypoints.Count - 1];
                var to = centerPoints[0];
                var fromDirection = (_waypoints[_waypoints.Count - 1] - _waypoints[_waypoints.Count - 2]).normalized * 4f;
                var toDirection = (centerPoints[0] - centerPoints[1]).normalized * 4f;

                _waypoints.Clear();

                const float count = 16;
                for (int i = 0; i < count + 1; i++)
                {
                    _waypoints.Add(GetBezierPosition(from, to, fromDirection, toDirection, i / count));
                    //Debug.DrawRay(_waypoints.Last(), Vector3.up * 2, Color.white, 1);
                }

                //_waypoints.Add(intersectionPoint);
            }

            //Reset
            _currentWayPoint = 0;

            //No need for ALL markers to be used, MINOR memory savings
            //For some reason the value at 0 in the ERRoad's center points is sometimes 0,0,0.  So ignoring that one.
            for (int i = 1; i < centerPoints.Length; i += 2)
            {
                _waypoints.Add(centerPoints[i]);
            }

            //Cleanup, just in case
            erConnectionData.Clear();
        }

        //This function is to find the center of the intersection and then make a point x% before that value.
        //This improves the auto's turn radius so the autos don't drive over sidewalks
        private Vector3 IntersectionVector(Vector3 start, Vector3 end)
        {
            Vector3 result = start + (end - start) * 0.2f;

            return result;
        }

        private Vector3 GetBezierPosition(Vector3 from, Vector3 to, Vector3 fromDirection, Vector3 toDirection, float t)
        {
            Vector3 p0 = from;
            Vector3 p1 = p0 + fromDirection;
            Vector3 p3 = to;
            Vector3 p2 = p3 + toDirection;

            // here is where the magic happens!
            return Mathf.Pow(1f - t, 3f) * p0 + 3f * Mathf.Pow(1f - t, 2f) * t * p1 + 3f * (1f - t) * Mathf.Pow(t, 2f) * p2 + Mathf.Pow(t, 3f) * p3;
        }

        private bool IsLinesIntersect(Vector2 line1point1, Vector2 line1point2, Vector2 line2point1, Vector2 line2point2)
        {

            Vector2 a = line1point2 - line1point1;
            Vector2 b = line2point1 - line2point2;
            Vector2 c = line1point1 - line2point1;

            float alphaNumerator = b.y * c.x - b.x * c.y;
            float betaNumerator = a.x * c.y - a.y * c.x;
            float denominator = a.y * b.x - a.x * b.y;

            if (denominator == 0)
            {
                return false;
            }
            else if (denominator > 0)
            {
                if (alphaNumerator < 0 || alphaNumerator > denominator || betaNumerator < 0 || betaNumerator > denominator)
                {
                    return false;
                }
            }
            else if (alphaNumerator > 0 || alphaNumerator < denominator || betaNumerator > 0 || betaNumerator < denominator)
            {
                return false;
            }
            return true;
        }
    }
}
