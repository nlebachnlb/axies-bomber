using System.Collections;
using System.Collections.Generic;
using Ability.Component;
using UnityEngine;

public class IroncladBarrier : AxieAbility<IroncladBarrierStats>
{
    [SerializeField] private IroncladBarrierStats defaultStats;
    [SerializeField] private Cooldown cooldown;

    private void Awake()
    {
        Stats = Instantiate(defaultStats);
    }

    public override bool CanDeploy()
    {
        return cooldown.IsAvailable;
    }

    public override void DeployAbility()
    {
        cooldown.StartCountdown();
    }
}
