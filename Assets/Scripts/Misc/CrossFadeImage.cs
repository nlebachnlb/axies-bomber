using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CrossFadeImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image image;

    private void Start()
    {
        CrossFade(0f, 0f);
    }

    public void CrossFade(float endValue, float duration)
    {
        image.CrossFadeAlpha(endValue, duration, true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CrossFade(0.8f, 0.15f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CrossFade(0f, 0.15f);
    }
}
