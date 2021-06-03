using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    [SerializeField] private CarDrive _car;

    [SerializeField] private Text _distance;
    [SerializeField] private Text _obstacle;
    [SerializeField] private Text _checkpoins;

    private int _distanceInt = 0;
    private int _obstacleInt = 0;
    private int _checkpointInt = 0;


    public int Distance => _distanceInt;
    public int Obstacle => _obstacleInt;
    public int Checkpoint => _checkpointInt;


    private void OnEnable()
    {
        _car.StatisticChanged += OnStatisticChanged;
    }

    private void OnStatisticChanged(int distance, int obstacle, int checkpoint)
    {
        _distanceInt = distance;
        _obstacleInt = obstacle;
        _checkpointInt = checkpoint;
        _distance.text = distance.ToString();
        _obstacle.text = obstacle.ToString();
        _checkpoins.text = checkpoint.ToString();
    }
}

