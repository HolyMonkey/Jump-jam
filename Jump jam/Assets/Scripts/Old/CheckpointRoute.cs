using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointRoute : MonoBehaviour
{
    [SerializeField] private Checkpoint[] _checkpoints;
    [SerializeField] private CarDrive _car;

    private bool flaresEnabled = false;

    private void Update()
    {
        StartCoroutine(ShowFlares());
    }

    private IEnumerator ShowFlares()
    {
        if (_car.Jumped && !flaresEnabled)
        {
            flaresEnabled = true;
            foreach (var checkpoint in _checkpoints)
            {
                yield return new WaitForSeconds(0.1f);
                checkpoint.gameObject.SetActive(true);
            }
        }
    }
}
