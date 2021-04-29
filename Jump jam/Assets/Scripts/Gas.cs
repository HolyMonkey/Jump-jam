using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gas : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private float _gasValue;
    [SerializeField] private float _gasDelta;
    [SerializeField] private SliderJoint2D _slider;
    
    public void OnDrag(PointerEventData eventData)
    {
        _gasValue += _gasDelta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
