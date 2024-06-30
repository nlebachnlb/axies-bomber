using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : MonoBehaviour
{
    [SerializeField] private Button buttonPlay;
    [SerializeField] private Button buttonUpgrade;
    [SerializeField] private List<AxieSlot> pickedSlots;

    public void AutoPick()
    {
        int[] ids = { 0, 3, 4 };
        for (int i = 0; i < 3; ++i)
        {
            AxiePackedConfig config = AppRoot.Instance.Config.availableAxies.GetAxiePackedConfigById(ids[i]);
            EventBus.RaiseOnPickAxie(i, config);
        }
    }

    private void Start()
    {
        if (AppRoot.Instance.Mode == AppRoot.PlayMode.TestPlayground)
        {
            AutoPick();
            OnSelectPlay();
        }
    }

    private void Awake()
    {
        buttonPlay.onClick.AddListener(OnSelectPlay);
        buttonUpgrade.onClick.AddListener(OnSelectUpgrade);
    }

    private void OnSelectPlay()
    {
        foreach (var slot in pickedSlots)
        {
            if (slot.ChosenAxie == null)
                return;
        }

        AppRoot.Instance.SoundManager.StopMenuBGM();
        AppRoot.Instance.SoundManager.PlayAudio(SoundManager.AudioType.Confirm);
        AppRoot.Instance.TransitionToScene(AppRoot.Instance.Config.playScene, true);
    }

    private void OnSelectUpgrade()
    {
        ViewController.Instance.upgradeUI.Open();
    }
}
