using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : MonoBehaviour
{
    [SerializeField] private Button buttonPlay;
    [SerializeField] private Button buttonUpgrade;
    [SerializeField] private List<AxieSlot> pickedSlots;

    private void Awake()
    {
        buttonPlay.onClick.AddListener(OnSelectPlay);
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
}
