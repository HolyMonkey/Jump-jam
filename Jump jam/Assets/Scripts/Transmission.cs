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

    private int _currentSpeed = 0;

    public event UnityAction<int> SpeedChanged;

    public int CurrentSpeed => _currentSpeed;
    public void ChangeSpeed()
    {
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
        for(int i = 5; i > 0; i--)
        {
            _knob.transform.position = _knobPosition[i-1].position;
            yield return new WaitForSeconds(0.7f);
        }
        yield break;
    }
}
