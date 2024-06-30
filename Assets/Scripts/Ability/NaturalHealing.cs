using System.Collections;
using System.Collections.Generic;
using Ability.Component;
using UnityEngine;

public class NaturalHealing : AxieAbility<NaturalHealingStats>
{
    [SerializeField] Cooldown cooldown;

    private AxieHeroDataHolder axieHeroDataHolder;

    public override void AssignOwner(GameObject owner)
    {
        base.AssignOwner(owner);
        axieHeroDataHolder = owner.GetComponent<AxieHeroDataHolder>();
    }

    public override bool CanDeploy()
    {
        return cooldown.IsAvailable;
    }

    public override void DeployAbility()
    {
        cooldown.StartCountdown(Stats.Cooldown);
        axieHeroDataHolder.Data.health += 1;
        axieHeroDataHolder.Data.RaiseUpdateInfo();

        if (Random.Range(0f, 1f) < Stats.AllyHealRate)
        {
            Debug.Log("[Ability] TODO: Heal ally HP");
        }
    }
}
