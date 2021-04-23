using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Result : MonoBehaviour
{
    [SerializeField] private CarDrive _car;
    [SerializeField] private TMP_Text _checkpointText;
    [SerializeField] private TMP_Text _obstacletText;
    [SerializeField] private TMP_Text _distanceText;
    [SerializeField] private TMP_Text _totalText;
    [SerializeField] private Statistics _stat;

    private void OnEnable()
    {
        _stat.gameObject.SetActive(false);  

        StartCoroutine(Show(_stat.Checkpoint, _checkpointText, 0.1f));
        StartCoroutine(Show(_stat.Distance, _distanceText, 0.001f));
        StartCoroutine(Show(_stat.Obstacle, _obstacletText, 0.1f));
        StartCoroutine(Show(_stat.Checkpoint * 20 + _stat.Distance * 2 - _stat.Obstacle * 10, _totalText, 0.001f));
    }


    private void View(TMP_Text text, int value)
    {
        text.text = value.ToString();
    }


    private IEnumerator Show(int value, TMP_Text text, float speed)
    {
        for (int i = 1; i <= value; i++)
        {
            yield return new WaitForSeconds(speed);
            text.text = i.ToString();
        }
        yield break;
    }
}
