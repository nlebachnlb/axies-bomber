using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHUD : MonoBehaviour
{
    [SerializeField] private Image abilityIcon;
    [SerializeField] private Image progress;
    [SerializeField] private TMPro.TextMeshProUGUI textProgress;

    public void SetProgress(float current, float max)
    {
        progress.fillAmount = current / max;
        textProgress.text = (current / max) * 100f + "%";
        if (current >= max)
            abilityIcon.color = Color.white;
        else
            abilityIcon.color = Color.gray;
    }

    public void SetIcon(Sprite sprite)
    {
        abilityIcon.sprite = sprite;
    }
}
