using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinyCatapult : AxieAbility<TinyCatapultStats>
{
    [SerializeField] private ParticleSystem fx;
    [SerializeField] private TinyCatapultKillField killField;

    private int state = 0;
    private float cooldownTime;
    private float sturdyTimer;
    private AxieHeroData axieData;
    private HealthController health;

    private void OnDestroy()
    {
        axieData.SetExtraParam(AxieHeroData.PARAM_COOLDOWN_TIME, cooldownTime);
    }

    private void Start()
    {
        killField.gameObject.SetActive(false);
        state = 0;
    }

    private void Update()
    {
        if (state == 0)
        {
            if (cooldownTime < Stats.cooldownTime)
            {
                cooldownTime += Time.deltaTime;
                RaiseOnCooldown(cooldownTime, Stats.cooldownTime);
                EventBus.RaiseOnAbilityCooldown(cooldownTime, Stats.cooldownTime, 2);
            }
        }
        else if (state == 1)
        {
            if (sturdyTimer > 0)
            {
                sturdyTimer -= Time.deltaTime;
                RaiseOnCooldown(cooldownTime, Stats.cooldownTime);
                EventBus.RaiseOnAbilityCooldown(sturdyTimer, Stats.sturdyDuration, 2);
                if (sturdyTimer < 0)
                {
                    sturdyTimer = 0f;
                    fx.Stop();
                    state = 0;
                    cooldownTime = 0f;
                    killField.gameObject.SetActive(false);
                }
            }
        }
    }

    public override void SetExtraParams(AxieHeroData axieHero)
    {
        base.SetExtraParams(axieHero);
        if (axieHero.ability != null)
            Stats = (TinyCatapultStats)Instantiate(axieHero.ability);
        cooldownTime = (int)axieHero.GetExtraParam(AxieHeroData.PARAM_COOLDOWN_TIME, Stats.cooldownTime);
        axieData = axieHero;
        RaiseOnCooldown(cooldownTime, Stats.cooldownTime);
        EventBus.RaiseOnAbilityCooldown(cooldownTime, Stats.cooldownTime, 1);
        Debug.Log("Extra param: " + cooldownTime);
    }

    public override void DeployAbility()
    {
        if (health == null)
            health = Owner.GetComponent<HealthController>();

        Debug.Log("TinyCatapult activated");
        RaiseOnCooldown(cooldownTime, Stats.cooldownTime);
        EventBus.RaiseOnAbilityCooldown(cooldownTime, Stats.cooldownTime, 2);
        sturdyTimer = Stats.sturdyDuration;
        health.SetInvincible(Stats.sturdyDuration);
        fx.Play();
        state = 1;
        killField.gameObject.SetActive(true);
    }

    public override bool CanDeploy()
    {
        return cooldownTime >= Stats.cooldownTime;
    }
}
