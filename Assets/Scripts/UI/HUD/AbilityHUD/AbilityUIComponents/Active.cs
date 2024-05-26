using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ability.UI
{
    /// <summary>
    /// Control when skill become active/inactive
    /// </summary>
    public class Active : AbilityUIComponent
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

        private void Activate()
        {
            mask.SetActive(false);
            animator.SetTrigger("Charged");
        }

        private void Deactivate()
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
            if (current >= max)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
        }
    }
}