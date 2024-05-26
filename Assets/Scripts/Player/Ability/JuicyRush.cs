using System.Collections;
using System.Collections.Generic;
using Ability.Component;
using UnityEngine;

public class JuicyRush : AxieAbility<JuicyRushStats>
{
    [SerializeField] private JuicyRushStats defaultStats;
    [SerializeField] private Cooldown cooldown;

    private void Awake()
    {
        Stats = Instantiate(defaultStats);
    }

    public override bool CanDeploy()
    {
        Debug.Log($"will deploy {cooldown.IsAvailable}");
        return cooldown.IsAvailable;
    }

    public override void DeployAbility()
    {
        Debug.Log($"please countdown");
        cooldown.StartCountdown();
    }
}
