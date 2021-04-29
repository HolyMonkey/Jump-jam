using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : MonoBehaviour
{
    [SerializeField] private int _speedValue;
    [SerializeField] private Transmission _transmission;
        
    public void SetNextSpeed()
    {
        _transmission.NextSpeed(_speedValue);
        _transmission.ChangeSpeed();
    }
}
