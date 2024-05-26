using System.Collections;
using System.Collections.Generic;
using Ability.Component;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ability.UI
{
    public class Cooldown : AbilityUIComponent
    {
        private AxieAbility ability;
        private Ability.Component.Cooldown cooldown;

        private GameObject mask;
        private TextMeshProUGUI textProgress;
        private Image progress;

        protected override void Awake()
        {
            base.Awake();

            ability = hud.currentAbility;
            textProgress = hud.textProgress;
            progress = hud.progress;
            mask = hud.mask.gameObject;

            cooldown = ability.GetComponent<Ability.Component.Cooldown>();
        }

        private void Update()
        {
            if (!cooldown.IsAvailable)
            {
                mask.SetActive(true);

                var remainingTime = cooldown.RemainingTime;
                var flooredRemainingTime = Mathf.FloorToInt(remainingTime);

                textProgress.text = flooredRemainingTime == 0 ? remainingTime.ToString("f1") : flooredRemainingTime.ToString();
                progress.fillAmount = cooldown.RemainingTimeAsPercentage;
            }
            else
            {
                mask.SetActive(false);
            }
        }
    }
}