using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    public UnityEvent onHighlighted;

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnSelect(BaseEventData eventData)
    {
        
    }
}
