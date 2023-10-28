using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientHunter : AxieAbility<PatientHunterStats>
{
    [SerializeField] private ParticleSystem fx;
    [SerializeField] private PatientHunterStats defaultStats;

    private int killedEnemies = 0;
    private AxieHeroData axieData;

    private void Awake()
    {
        Stats = Instantiate(defaultStats);
        EventBus.onEnemyDeath += OnEnemyDeath;
    }

    private void OnDestroy()
    {
        EventBus.onEnemyDeath -= OnEnemyDeath;
        axieData.SetExtraParam("killedEnemies", killedEnemies);
    }

    private void Start()
    {
    }

    public override void SetExtraParams(AxieHeroData axieHero)
    {
        base.SetExtraParams(axieHero);
        Stats = (PatientHunterStats)Instantiate(axieHero.ability);
        killedEnemies = (int)axieHero.GetExtraParam("killedEnemies", Stats.killsNeeded);
        axieData = axieHero;
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
        EventBus.RaiseOnAbilityCooldown(killedEnemies, Stats.killsNeeded, 1);
    }

    private HealthController healthController;
}