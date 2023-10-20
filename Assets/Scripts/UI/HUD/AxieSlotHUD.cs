using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AxieSlotHUD : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI textBomb, textHealth;
    [SerializeField] private Image axieIcon;
    [SerializeField] private TMPro.TextMeshProUGUI textKey;

    public void SetInfo(int bomb, int health)
    {
        textBomb.text = "" + bomb;
        textHealth.text = "" + health;
    }

    public void SetAxieIcon(Sprite axie)
    {
        axieIcon.sprite = axie;
    }

    public void SetKeyInput(KeyCode key)
    {
        textKey.text = key.ToString().Replace("Alpha", "");
    }
}
