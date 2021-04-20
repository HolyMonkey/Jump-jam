using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UiWheel : MonoBehaviour
{
    [SerializeField] private Car _car;    

    public IEnumerator Rotate(int angle)
    {
        transform.DORotate(new Vector3(0, 0, angle), 0.2f);
        yield return new WaitForSeconds(0.2f);

        transform.DORotate(new Vector3(0, 0, 0), 0.2f);
        yield return new WaitForSeconds(0.2f);
        yield break;
    }
}
