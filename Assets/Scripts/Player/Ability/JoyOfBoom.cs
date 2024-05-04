using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyOfBoom : AxieAbility<JoyOfBoomStats>, IBuff
{
    [SerializeField] private EnemyKillTracker enemyKillTracker;
    [SerializeField] private JoyOfBoomStats defaultStats;

    public float BuffDamage => enemyKillTracker.killedEnemies * Stats.damageBuffPerKill;

    private void Awake()
    {
        Stats = Instantiate(defaultStats);
    }

    public override bool CanDeploy()
    {
        return true;
    }

    public override void DeployAbility()
    {
        base.DeployAbility();
    }
}
