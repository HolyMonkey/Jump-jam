using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardRotateAssist : MonoBehaviour
{
    [SerializeField] private Drive _car;

    private void FixedUpdate()
    {
        if(_car.Jumped)
        {
            return;
        }

        Assist();
    }

    private void Assist()
    {        
        if (Mathf.Abs(transform.rotation.y) * Mathf.Rad2Deg > 0.1f)
        {            
            transform.Rotate(Vector3.up, 10* Mathf.Lerp(-transform.rotation.y, 0, Time.fixedDeltaTime));
        }
    }
}
