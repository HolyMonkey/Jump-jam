using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardRotateAssist : MonoBehaviour
{
    private void FixedUpdate()
    {
        Assist();
    }

    private void Assist()
    {
        Debug.Log(Mathf.Abs(transform.rotation.y) * Mathf.Rad2Deg);
        if (Mathf.Abs(transform.rotation.y) * Mathf.Rad2Deg > 3f)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, Mathf.Lerp(transform.rotation.y, 0, Time.fixedDeltaTime), transform.rotation.z);
        }
    }
}
