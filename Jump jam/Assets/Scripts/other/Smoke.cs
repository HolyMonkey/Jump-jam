using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    [SerializeField] private GasPedal _pedal;    

    private Quaternion _rotation = Quaternion.Euler(-180, 0, 0);

    void Update()
    {
        transform.rotation = _rotation;
    }
}
