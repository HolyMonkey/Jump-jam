using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Result : MonoBehaviour
{
    [SerializeField] private Drive _car;
    [SerializeField] private TMP_Text _checkpointText;
    [SerializeField] private TMP_Text _obstacletText;
    [SerializeField] private TMP_Text _distanceText;
    [SerializeField] private TMP_Text _totalText;
    [SerializeField] private Statistics _stat;
    [SerializeField] GameObject _wheel;

    private void OnEnable()
    {
        //_stat.gameObject.SetActive(false);
        //StartCoroutine(Hide());
        //StartCoroutine(Show(_stat.Checkpoint, _checkpointText, 0.1f));
        StartCoroutine(Show(Mathf.RoundToInt(_car.transform.position.z), _distanceText, 0.001f));
        StartCoroutine(Show(Mathf.RoundToInt(_car.CarSmashed/4), _obstacletText, 0.1f));
        StartCoroutine(Show(Mathf.RoundToInt(_car.transform.position.z) + Mathf.RoundToInt(_car.CarSmashed / 4) * 10, _totalText, 0.001f));
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

    public IEnumerator Hide()
    {
        _wheel.transform.DOScale(new Vector3(0.01f, 0.01f, 1), 0.4f);
        //_wheel.transform.DOMove(_target.position, 1f);
        yield return new WaitForSeconds(0.4f);
        _wheel.gameObject.SetActive(false);
    }
}
