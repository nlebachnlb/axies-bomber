using System.Collections;
using System.Collections.Generic;
using Ability.Component;
using UnityEngine;

public class NaturalHealing : AxieAbility<NaturalHealingStats>
{
    [SerializeField] Cooldown cooldown;

    public Cooldown Cooldown => cooldown;

    public override bool CanDeploy()
    {
        return cooldown.IsAvailable;
    }

    public override void DeployAbility()
    {
        cooldown.StartCountdown(Stats.Cooldown);
        controller.AxieHeroData.health += 1;
        controller.AxieHeroData.RaiseUpdateInfo();

        if (Random.Range(0f, 1f) < Stats.AllyHealRate)
        {
            Debug.Log("[Ability] TODO: Heal ally HP");
        }
    }
}
