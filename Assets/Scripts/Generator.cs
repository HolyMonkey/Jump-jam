using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class Generator : MonoBehaviour
    {
        private Coin[] _coins = null;

        [SerializeField] private Road _roadPrefab = null;
        [SerializeField] private Coin _coinPrefab = null;
        [SerializeField] private Material _silverCoinMaterial = null;

        [SerializeField] private Transform _roadsRoot = null;
        [SerializeField] private Transform _coinsRoot = null;

        [SerializeField] private float _roadLength = 100;
        [SerializeField, Min(3)] private int _maxCoinsCount = 30;
        [SerializeField] private float _coinsPositionInterval = 2;

        private Road _firstRoad = null;
        private Road _secondRoad = null;

        private void Start()
        {
            _coins = new Coin[_maxCoinsCount];

            _firstRoad = Instantiate(_roadPrefab, _roadsRoot);
            _secondRoad = Instantiate(_roadPrefab, _firstRoad.transform.position + Vector3.forward * _roadLength, Quaternion.identity, _roadsRoot);

            _firstRoad.name = "First Road";
            _secondRoad.name = "Second Road";

            _firstRoad.GenerationRequested += OnRoadGenerationRequested;
            _secondRoad.GenerationRequested += OnRoadGenerationRequested;

            var position = _coinsRoot.position + Vector3.up * 2;
            position.x = (Random.value > 0.5 ? 1 : -1) * 2.5f;

            _coins[0] = Instantiate(_coinPrefab, position, _coinPrefab.transform.rotation, _coinsRoot);
            _coins[0].GenerationRequested += OnCoinGenerationRequested;

            for (int i = 1; i < _maxCoinsCount; i++)
            {
                position = _coins[i - 1].transform.position + Vector3.forward * _coinsPositionInterval;
                position.x = (Random.value > 0.5 ? 1 : -1) * 2.5f;

                _coins[i] = Instantiate(_coinPrefab, position, _coinPrefab.transform.rotation, _coinsRoot);
                _coins[i].GenerationRequested += OnCoinGenerationRequested;

                if (i % (_maxCoinsCount / 100.0 * 10) == 0)
                {
                    _coins[i].SetMaterial(_silverCoinMaterial);
                }
            }
        }

        private void OnRoadGenerationRequested()
        {
            var temp = _firstRoad;
            _firstRoad = _secondRoad;
            _secondRoad = temp;

            _secondRoad.transform.position = _firstRoad.transform.position + Vector3.forward * _roadLength;
        }

        private void OnCoinGenerationRequested(Coin sender)
        {
            int senderIndex = -1;

            for (int i = 0; i < _maxCoinsCount; i++)
            {
                if (sender == _coins[i])
                {
                    senderIndex = i;
                }
            }

            var position = _coins[_maxCoinsCount - 1].transform.position + Vector3.forward * _coinsPositionInterval;
            position.x = (Random.value > 0.5 ? 1 : -1) * 2.5f;
            sender.transform.position = position;

            for (int i = senderIndex; i < _maxCoinsCount - 1; i++)
            {
                _coins[i] = _coins[i + 1];
            }

            _coins[_maxCoinsCount - 1] = sender;
        }
    }
}