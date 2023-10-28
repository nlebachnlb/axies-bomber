using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : MonoBehaviour
{
    [SerializeField] private Button buttonPlay;
    [SerializeField] private Button buttonUpgrade;

    private void Awake()
    {
        buttonPlay.onClick.AddListener(OnSelectPlay);
    }

    private void OnSelectPlay()
    {
        AppRoot.Instance.SoundManager.StopMenuBGM();
        AppRoot.Instance.SoundManager.PlayAudio(SoundManager.AudioType.Confirm);
        AppRoot.Instance.TransitionToScene(AppRoot.Instance.Config.playScene, true);
    }
}
