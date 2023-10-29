using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AbilityHUD : MonoBehaviour
{
    [SerializeField] private Image abilityIcon;
    [SerializeField] private Image progress;
    [SerializeField] private TMPro.TextMeshProUGUI textProgress;
    [SerializeField] private Image mask;

    private CanvasGroup group;
    private Animator animator;

    private void Awake()
    {
        EventBus.onAbilityCooldown += OnAbilityCooldown;
        EventBus.onSwitchAxieHero += OnSwitchHero;

        group = GetComponent<CanvasGroup>();
        animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        EventBus.onAbilityCooldown -= OnAbilityCooldown;
        EventBus.onSwitchAxieHero -= OnSwitchHero;
    }

    public void SetProgress(float current, float max, int displayType)
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
            case 0:
                textProgress.text = (current / max) * 100f + "%";
                break;
            case 1:
                textProgress.text = current + "/" + max;
                break;
            case 2:
                textProgress.text = (int)(max - current) + "s";
                break;
        }
    }

    public void SetIcon(Sprite sprite)
    {
        abilityIcon.sprite = sprite;
    }

    private void OnAbilityCooldown(float current, float max, int displayType)
    {
        SetProgress(current, max, displayType);
    }

    private void OnSwitchHero(AxieHeroData heroData)
    {
        if (heroData.ability != null)
        {
            SetIcon(heroData.ability.targetAxie);
            group.alpha = 1f;
        }
        else
        {
            group.alpha = 0f;
        }
    }
}
