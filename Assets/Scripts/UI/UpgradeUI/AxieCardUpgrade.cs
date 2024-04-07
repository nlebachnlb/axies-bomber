using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AxieCardUpgrade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private AxieCardDisplay axieCardDisplay;
    [SerializeField] private GameObject selected;
    [SerializeField] private Button buttonSelect;

    private AxiePackedConfig axiePackedConfig;

    public int Id => axieCardDisplay.Id;

    public void Init(AxiePackedConfig axiePackedConfig, UnityAction onSelected)
    {
        this.axiePackedConfig = axiePackedConfig;
        buttonSelect.onClick.AddListener(onSelected);
    }

    public void Load()
    {
        axieCardDisplay.Init(axiePackedConfig);
        axieCardDisplay.Load();
    }

    public void Select() => selected.SetActive(true);

    public void Deselect() => selected.SetActive(false);

    public void OnPointerEnter(PointerEventData eventData)
    {
        axieCardDisplay.transform.DOLocalMoveY(32f, 0.3f).SetEase(Ease.OutCirc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        axieCardDisplay.transform.DOLocalMoveY(0f, 0.3f).SetEase(Ease.OutCirc);
    }
}
