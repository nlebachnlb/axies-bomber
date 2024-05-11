using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityActive : AbilityHUDComponent
{
    public Image progress;
    public TextMeshProUGUI textProgress;
    public GameObject mask;
    public Animator animator;
    public AxieAbility ability;

    protected override void Awake()
    {
        base.Awake();

        progress = hud.progress;
        textProgress = hud.textProgress;
        mask = hud.mask.gameObject;
        animator = hud.animator;
        ability = hud.currentAbility;

        ability.OnCooldown += OnAbilityCooldown;
    }

    private void OnDestroy()
    {
        ability.OnCooldown -= OnAbilityCooldown;
    }

    private void Active()
    {
        mask.SetActive(false);
        animator.SetTrigger("Charged");
    }

    private void Inactive()
    {
        mask.SetActive(true);
        animator.SetTrigger("Cooldown");
    }

    private void OnAbilityCooldown(float current, float max, int type)
    {
        SetProgress(current, max, ability.DisplayType);
    }

    private void SetProgress(float current, float max, SkillConfig.DisplayType displayType)
    {
        progress.fillAmount = current / max;
        if (current >= max)
        {
            Active();
        }
        else
        {
            Inactive();
        }

        switch (displayType)
        {
            case SkillConfig.DisplayType.Percentage:
                textProgress.text = $"{progress.fillAmount * 100f}%";
                break;
            case SkillConfig.DisplayType.CurrentOverMax:
                textProgress.text = $"{current}/{max}";
                break;
            case SkillConfig.DisplayType.Seconds:
                textProgress.text = $"{max - current} s";
                break;
        }
    }
}
