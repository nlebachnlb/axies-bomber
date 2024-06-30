using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientHunter : AxieAbility<PatientHunterStats>
{
    [SerializeField] private ParticleSystem fx;

    private int killedEnemies = 0;
    private AxieHeroData axieData;

    protected override void Awake()
    {
        EventBus.onEnemyDeath += OnEnemyDeath;
    }

    private void OnDestroy()
    {
        EventBus.onEnemyDeath -= OnEnemyDeath;
        axieData.SetExtraParam(AxieHeroData.PARAM_KILLED_ENEMIES, killedEnemies);
    }

    public override void SetExtraParams(AxieHeroData axieHero)
    {
        base.SetExtraParams(axieHero);
        if (axieHero.ability != null)
            Stats = (PatientHunterStats)Instantiate(axieHero.ability);
        killedEnemies = (int)axieHero.GetExtraParam(AxieHeroData.PARAM_KILLED_ENEMIES, Stats.killsNeeded);
        axieData = axieHero;
        RaiseOnCooldown(killedEnemies, Stats.killsNeeded);
        EventBus.RaiseOnAbilityCooldown(killedEnemies, Stats.killsNeeded, 1);
        Debug.Log("Extra param: " + killedEnemies);
    }

    public override void DeployAbility()
    {
        Debug.Log("PatientHunter activated");
        if (healthController == null)
            healthController = Owner.GetComponent<HealthController>();

        StartCoroutine(DeployProgress());
        killedEnemies = 0;
        RaiseOnCooldown(killedEnemies, Stats.killsNeeded);
        EventBus.RaiseOnAbilityCooldown(killedEnemies, Stats.killsNeeded, 1);
    }

    public override bool CanDeploy()
    {
        return killedEnemies >= Stats.killsNeeded;
    }

    private IEnumerator DeployProgress()
    {
        healthController.SetInvincible(Stats.dismissDuration);
        fx.Play();
        yield return new WaitForSeconds(Stats.dismissDuration);
        fx.Stop();
    }

    private void OnEnemyDeath()
    {
        killedEnemies++;
        if (killedEnemies >= Stats.killsNeeded)
            IsCooldown = true;
        RaiseOnCooldown(killedEnemies, Stats.killsNeeded);
        EventBus.RaiseOnAbilityCooldown(killedEnemies, Stats.killsNeeded, 1);
    }

    private HealthController healthController;
}