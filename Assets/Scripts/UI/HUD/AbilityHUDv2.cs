using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHUDv2 : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private Animator animator;
    [SerializeField] private Image abilityIcon;
    [SerializeField] private Image progress;
    [SerializeField] private TextMeshProUGUI textProgress;
    [SerializeField] private TextMeshProUGUI textDeploymentKey;
    [SerializeField] private Image mask;

    private AxieAbility currentAbility;

    public void Assign(SkillType skillType, AxieAbility axieAbility)
    {
        Clear();
        currentAbility = axieAbility;
        currentAbility.OnCooldown += OnAbilityCooldown;

        if (AppRoot.Instance.Config.inputSettings.skillDeploymentKeys.TryGetValue(skillType, out var keyCode))
            textDeploymentKey.text = keyCode.ToString();
    }

    private void OnDestroy()
    {
        Clear();
    }

    private void Clear()
    {
        if (currentAbility != null)
            currentAbility.OnCooldown -= OnAbilityCooldown;
    }

    private void OnAbilityCooldown(float current, float max, int type)
    {
        SetProgress(current, max, currentAbility.DisplayType);
    }

    private void SetProgress(float current, float max, SkillConfig.DisplayType displayType)
    {
        progress.fillAmount = current / max;
        if (current >= max)
        {
            animator.SetTrigger("Charged");
            mask.gameObject.SetActive(false);
        }
        else
        {
            mask.gameObject.SetActive(true);
            animator.SetTrigger("Cooldown");
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
