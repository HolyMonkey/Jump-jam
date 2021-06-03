using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour
{
    [SerializeField] private GameObject _frame;

    public void ShowFrame()
    {
        _frame.SetActive(true);
    }
}
