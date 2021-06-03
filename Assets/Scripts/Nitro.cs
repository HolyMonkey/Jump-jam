using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Nitro : MonoBehaviour
{
    [SerializeField] private Drive _car;
    [SerializeField] private GameObject _nitro;
    [SerializeField] private ParticleSystem _smoke;    
    [SerializeField] private ParticleSystem _smoke2;    
    

    public void Boost()
    {
        _car.BoostValue++;
        _nitro.SetActive(true);
        _smoke.startLifetime = Mathf.Lerp(_smoke.startLifetime, 0, 0.8f);
        _smoke2.startLifetime = Mathf.Lerp(_smoke2.startLifetime, 0, 0.8f);
    }
}
