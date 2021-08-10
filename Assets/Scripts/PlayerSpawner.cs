using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private Transform[] _spawnPoints;

        private void Awake()
        {
            Time.timeScale = 1;
        }

        private void Start()
        {
            _player.position = _spawnPoints[GetRandomIndex()].position;
        }

        private int GetRandomIndex()
        {
            int randomIndex = Random.Range(0, _spawnPoints.Length - 1);
            return randomIndex;
        }
    }
}
