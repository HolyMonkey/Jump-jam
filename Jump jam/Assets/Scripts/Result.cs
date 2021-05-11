using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] private Drive _car;    
    [SerializeField] private Text _obstacletText;
    [SerializeField] private Text _distanceText;
    [SerializeField] private Text _totalText;
    [SerializeField] private Statistics _stat;
    [SerializeField] GameObject _wheel;
    [SerializeField] private ParticleSystem _boost;
    [SerializeField] private ParticleSystem _boost2;

    private void OnEnable()
    {
        //_stat.gameObject.SetActive(false);
        //StartCoroutine(Hide());
        //StartCoroutine(Show(_stat.Checkpoint, _checkpointText, 0.1f));
        StartCoroutine(Show(Mathf.RoundToInt(_car.transform.position.z), _distanceText, 0.001f));
        StartCoroutine(Show(Mathf.RoundToInt(_car.CarSmashed/4), _obstacletText, 0.1f));
        StartCoroutine(Show(Mathf.RoundToInt(_car.transform.position.z) + Mathf.RoundToInt(_car.CarSmashed / 4) * 10, _totalText, 0.001f));
       // _car.Brake(5000);
    }

    private void Update()
    {
        _boost.startLifetime = Mathf.Lerp(_boost.startLifetime, 0, 0.001f);
        _boost2.startLifetime = Mathf.Lerp(_boost2.startLifetime, 0, 0.001f);

        if(_boost.startLifetime < 0.03)
        {
            _boost.gameObject.SetActive(false);
            _boost2.gameObject.SetActive(false);
        }
    }

    private void View(Text text, int value)
    {
        text.text = value.ToString();
    }


    private IEnumerator Show(int value, Text text, float speed)
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
