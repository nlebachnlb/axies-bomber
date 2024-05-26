using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Ability.UI
{
    public class KilledEnemiesCounter : AbilityUIComponent
    {
        public TextMeshProUGUI text;
        public AxieAbility ability;

        private IEnemyKillTrackBehaviour enemyKillTrackBehaviour;

        protected override void Awake()
        {
            base.Awake();

            text = hud.textAuxilliary;
            ability = hud.currentAbility;
            enemyKillTrackBehaviour = ability as IEnemyKillTrackBehaviour;

            ability.OnAbilityUpdated += OnAbilityUpdated;
        }

        private void OnEnable()
        {
            text.gameObject.SetActive(true);
            SetText();
        }

        private void OnDisable()
        {
            text.gameObject.SetActive(false);
            text.text = "";
        }

        private void OnDestroy()
        {
            ability.OnAbilityUpdated -= OnAbilityUpdated;
        }

        private void OnAbilityUpdated()
        {
            SetText();
        }

        private void SetText()
        {
            text.text = enemyKillTrackBehaviour.KilledEnemies.ToString();
        }
    }
}