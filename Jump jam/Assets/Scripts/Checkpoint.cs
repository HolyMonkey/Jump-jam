using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void Fade()
    {
        _animator.SetTrigger("Taken");
    }
}
