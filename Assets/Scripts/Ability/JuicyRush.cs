using System.Collections;
using System.Collections.Generic;
using Ability.Component;
using UnityEngine;

public class JuicyRush : AxieAbility<JuicyRushStats>
{
    [SerializeField] private Timer timer;

    public Timer Timer => timer;

    private bool isTimerSet;
    private float effectDurationTimer = 0;

    private void Update()
    {
        if (!isTimerSet)
            return;

        if (effectDurationTimer < 0)
        {
            isTimerSet = false;
            effectDurationTimer = 0;

            var movementController = Owner.GetComponent<MovementController>();
            movementController.SetSpeedMultiplier(1);
            timer.StartCountdown(3);
        }
        else
        {
            effectDurationTimer -= Time.deltaTime;
        }
    }

    public override bool CanDeploy()
    {
        return timer.IsAvailable;
    }

    public override void DeployAbility()
    {
        if (!isTimerSet)
        {
            var movementController = Owner.GetComponent<MovementController>();
            movementController.SetSpeedMultiplier(1 + Stats.speedBuffPercentage);

            isTimerSet = true;
            effectDurationTimer = Stats.effectDuration;
        }
    }
}
