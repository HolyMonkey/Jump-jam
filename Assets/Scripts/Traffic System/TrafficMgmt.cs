using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyRoads3Dv3;
using System;

namespace JumpJam.TrafficSystem
{
    public class TrafficMgmt : MonoBehaviour
    {
        [SerializeField] private float _minHeight = 1;
        [SerializeField] private float _maxHeight = 2;
        [SerializeField] private int _autoCount = 1;
        [SerializeField] private int _autosPerRoad = 1;
        [SerializeField] private List<AutoMgmt> _autoPrefabs = new List<AutoMgmt>();

        private ERRoad[] _roads = null;

        void Start()
        {
            //UnityEngine.Random.InitState(10);

            var roadNetwork = new ERRoadNetwork();
            _roads = roadNetwork.GetRoads();

            for (int i = 0; i < _autoCount; i++)
            {
                var road = GetRoad();
                var centerPoints = road.GetSplinePointsCenter();

                //Somehow some roads come back with ZERO points
                if (centerPoints.Length > 0)
                {
                    for (int j = 0; j < _autosPerRoad && i < _autoCount && centerPoints.Length > 3; i++, j++)
                    {
                        var pointsCount = centerPoints.Length / 2 - 1;
                        var prefabNumber = UnityEngine.Random.Range(0, _autoPrefabs.Count);
                        var pointNumber = UnityEngine.Random.Range(1, pointsCount) * 2;
                        var autoPrefab = _autoPrefabs[prefabNumber];
                        var auto = Instantiate(autoPrefab);
                        var yPosition = UnityEngine.Random.Range(_minHeight, _maxHeight);
                        var xPosition = UnityEngine.Random.Range(auto.MinRight, auto.MaxRight);

                        auto.gameObject.name = "Auto " + i;
                        auto.enabled = false;
                        auto.PrevRoad = road;
                        auto.CurrentRoad = road;
                        auto.HoverAuto.transform.localPosition = new Vector3(xPosition, yPosition, 0);

                        auto.transform.position = centerPoints[pointNumber];

                        var points = road.GetSplinePointsCenter();
                        float distanceStart = Vector3.Distance(points[0], auto.transform.position);
                        float distanceEnd = Vector3.Distance(points[points.Length - 1], auto.transform.position);

                        //auto.CurrentWayPoint = pointNumber / 2 + 1;
                        var targetPointNumber = pointNumber + 1;
                        if (distanceStart > distanceEnd)
                        {
                            //auto.CurrentWayPoint -= 2;
                            targetPointNumber -= 2;
                        }

                        var toTarget = centerPoints[targetPointNumber] - auto.transform.position;

                        /*/
                        Debug.Log("CPL: " + centerPoints.Length);
                        Debug.Log("PN: " + pointNumber);
                        Debug.DrawRay(auto.transform.position, Vector3.up * 10, Color.magenta, 120);
                        Debug.DrawRay(centerPoints[pointNumber + 1], Vector3.up * 5, Color.yellow, 120);
                        Debug.DrawRay(centerPoints[pointNumber - 1], Vector3.up * 7.5f, Color.red, 120);
                        //*/
                        if (toTarget != Vector3.zero)  //to avoid the "Look rotation viewing vector is zero" exception
                        {
                            auto.transform.rotation = Quaternion.LookRotation(toTarget);
                        }
                        //Debug.DrawLine(auto.HoverAuto.position, auto.transform.position + Vector3.up * 10, Color.cyan, 120);
                        //auto.transform.forward = road.GetLookatSmooth(road.GetLength() / pointsCount * pointNumber, 0);
                        auto.enabled = true;  //Insure this is set AFTER the road and other values are set.  This value is set OFF in the prefab

                        centerPoints = Remove(centerPoints, pointNumber);
                    }
                }
            }
        }

        private T[] Remove<T>(T[] currentArray, int index)
        {
            var newArray = new T[currentArray.Length - 1];

            int i = 0;
            int j = 0;
            while (i < currentArray.Length)
            {
                if (i != index)
                {
                    newArray[j] = currentArray[i];
                    j++;
                }

                i++;
            }

            return newArray;
        }

        private ERRoad GetRoad()
        {
            var road = _roads[0]; //Just to insure that SOMETHING goes back

            ERConnection erConnection = null;

            while (erConnection == null)
            {
                var i = UnityEngine.Random.Range(0, _roads.Length);
                road = _roads[i];
                _roads = Remove(_roads, i);  //remove road from list, to avoid adding autos/drones to the same road

                erConnection = road.GetConnectionAtEnd();
            }

            return road;
        }
    }
}
