using Ability.Component;
using UnityEngine;

public class NaturalHealing : AxieAbility<NaturalHealingStats>
{
    [SerializeField] Timer cooldown;
    [SerializeField] ParticleSystem effect;

    public Timer Timer => cooldown;

    public override bool CanDeploy()
    {
        return cooldown.IsAvailable;
    }

    public override void DeployAbility()
    {
        cooldown.StartCountdown(Stats.Cooldown);
        controller.AxieHeroData.health += (int)Stats.SelfHealHp;
        controller.AxieHeroData.RaiseUpdateInfo();
        effect.Play();

        if (Random.Range(0f, 1f) < Stats.AllyHealRate)
        {
            Debug.Log("[Ability] TODO: Heal ally HP");
        }
    }
}
