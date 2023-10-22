using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AxieCard : MonoBehaviour
{
    [Header("Figure")]
    [SerializeField] private Image axieIcon;
    [SerializeField] private TMPro.TextMeshProUGUI textAxieName;

    [Header("Basic Stats")]
    [SerializeField] private TMPro.TextMeshProUGUI textSpeed;
    [SerializeField] private TMPro.TextMeshProUGUI textHp;
    [SerializeField] private TMPro.TextMeshProUGUI textBomb;
    [SerializeField] private TMPro.TextMeshProUGUI textLength;

    [HideInInspector]
    public AxiePackedConfig config;

    public void ReloadConfig()
    {
        if (config == null)
            return;

        axieIcon.sprite = config.axieConfig.icon;
        textAxieName.text = config.axieConfig.axieName;
        textSpeed.text = "" + config.axieStats.speed;
        textHp.text = "" + config.axieStats.health;
        textBomb.text = "" + config.axieStats.bombMagazine;
        textLength.text = "" + config.axieStats.bombExplosionRadius;
    }
}
