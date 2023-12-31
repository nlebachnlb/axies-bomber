using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class SkillItem : MonoBehaviour
{
    public SkillConfig Config { get; set; }

    [SerializeField] private Image card;
    [SerializeField] private TMPro.TextMeshProUGUI textSkillName;
    [SerializeField] private TMPro.TextMeshProUGUI textSkillDesc;
    [SerializeField] private TMPro.TextMeshProUGUI textSkillMinorDesc;
    [SerializeField] private GameObject abilityMark;
    [SerializeField] private Image axie;
    [SerializeField] private GameObject abilityExplanation;
    [SerializeField] private Image select;
    [SerializeField] private Color maxLevelColor;

    private void Start()
    {
        select.CrossFadeAlpha(0, 0, true);
    }

    public void Reload()
    {
        card.sprite = Config.targetAxie;
        textSkillName.text = Config.skillName + (Config.level != -1 ? (" - Lv." + (Config.level + 1)) : " - MAX");
        textSkillDesc.text = Config.GenerateDescription();
        textSkillMinorDesc.text = Config.GenerateMinorDescription();
        abilityMark.SetActive(Config.isAbility);
        axie.sprite = Config.ownerAxie.icon;
        abilityExplanation.SetActive(Config.isAbility);

        if (Config.level == -1)
            textSkillName.color = maxLevelColor;
    }

    public void OnPointerEnter(BaseEventData data)
    {
        if (Config && Config.level == -1)
        {
            return;
        }

        select.CrossFadeAlpha(0.8f, 0.15f, true);
        card.rectTransform.DOLocalMoveX(-337.32f - 16f, 0.15f);
    }

    public void OnPointerExit(BaseEventData data)
    {
        select.CrossFadeAlpha(0f, 0.15f, true);
        card.rectTransform.DOLocalMoveX(-337.32f, 0.15f);
    }

    public void OnPointerClick(BaseEventData data)
    {
        if (Config && Config.level == -1)
        {
            return;
        }
        EventBus.RaiseOnPickSkill(Config);
    }
}
