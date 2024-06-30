using System.Collections;
using System.Collections.Generic;
using Ability.Component;
using UnityEngine;

public class JuicyRush : AxieAbility<JuicyRushStats>
{
    [SerializeField] private Cooldown cooldown;

    public Cooldown Cooldown => cooldown;

    private bool isTimerSet;
    private float timer = 0;

    private void Update()
    {
        if (!isTimerSet)
            return;

        if (timer < 0)
        {
            isTimerSet = false;
            timer = 0;

            var movementController = Owner.GetComponent<MovementController>();
            movementController.SetSpeedMultiplier(1);
            cooldown.StartCountdown(3);
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public override bool CanDeploy()
    {
        return cooldown.IsAvailable;
    }

    public override void DeployAbility()
    {
        if (!isTimerSet)
        {
            var movementController = Owner.GetComponent<MovementController>();
            movementController.SetSpeedMultiplier(1 + Stats.speedBuffPercentage);

            isTimerSet = true;
            timer = Stats.effectDuration;
        }
    }
}
