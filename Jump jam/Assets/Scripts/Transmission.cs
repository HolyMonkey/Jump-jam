using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Transmission : MonoBehaviour
{
    [SerializeField] private Image _knob;
    [SerializeField] private Transform[] _knobPosition;
    [SerializeField] private Tachometer _tachometer;
    [SerializeField] private Image _image;

    private int _currentSpeed = 0;
    private Color _currentColor;

    public event UnityAction<int> SpeedChanged;

    public int CurrentSpeed => _currentSpeed;

    private void Start()
    {
        _currentColor = _image.color;
    }

    public void ChangeSpeed()
    {
        StartCoroutine(Click());
        _knob.gameObject.SetActive(true);

        if (_currentSpeed < 5)
        {
            _knob.transform.position = _knobPosition[_currentSpeed].position;
            _currentSpeed++;
            SpeedChanged?.Invoke(1);
        }
    }

    public IEnumerator LoseSpeed()
    {
        for (int i = 5; i > 0; i--)
        {
            _knob.transform.position = _knobPosition[i - 1].position;
            yield return new WaitForSeconds(0.7f);
        }
        yield break;
    }

    public IEnumerator Click()
    {
        //_image.color = new Color(199, 199, 199, 1);
        _image.transform.localScale = new Vector3(2.35f, 2.35f, 1);
        yield return new WaitForSeconds(0.15f);
        //_image.color = _currentColor;
        _image.transform.localScale = new Vector3(2.2f, 2.2f, 1);
        yield break;
    }
}
