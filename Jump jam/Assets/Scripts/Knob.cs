using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knob : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void Alert()
    {
        _animator.SetTrigger("Alert");
    }

    public void StopAlert()
    {
        _animator.SetTrigger("Stop");
    }
}
