using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AxieCardDisplay : MonoBehaviour
{
    [SerializeField] private Image axieIcon;
    [SerializeField] private TextMeshProUGUI textAxieName;
    [SerializeField] private Sprite classTank;
    [SerializeField] private Sprite classAssassin;
    [SerializeField] private Sprite classDps;
    [SerializeField] private Image imgClass;

    [Header("Basic Stats")]
    [SerializeField] private TextMeshProUGUI textSpeed;
    [SerializeField] private TextMeshProUGUI textHp;
    [SerializeField] private TextMeshProUGUI textBomb;
    [SerializeField] private TextMeshProUGUI textLength;

    private AxiePackedConfig config;

    public int Id => config.id;

    public void Init(AxiePackedConfig axiePackedConfig)
    {
        config = axiePackedConfig;
    }

    public void Load()
    {
        if (config == null)
            return;

        axieIcon.sprite = config.axieConfig.icon;
        textAxieName.text = config.axieConfig.axieName;
        textSpeed.text = "" + config.axieStats.speed;
        textHp.text = "" + config.axieStats.health;
        textBomb.text = "" + config.axieStats.bombMagazine;
        textLength.text = "" + config.axieStats.bombExplosionRadius;

        switch (config.axieConfig.axieClass)
        {
            case AxieClass.Damage:
                imgClass.sprite = classDps;
                break;
            case AxieClass.Assassin:
                imgClass.sprite = classAssassin;
                break;
            case AxieClass.Tanker:
                imgClass.sprite = classTank;
                break;
        }
    }
}
