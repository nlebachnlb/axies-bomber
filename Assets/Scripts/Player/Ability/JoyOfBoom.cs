using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyOfBoom : AxieAbility<JoyOfBoomStats>, IBuff, IEnemyKillTrackBehaviour
{
    [SerializeField] private JoyOfBoomStats defaultStats;
    public EnemyKillTracker enemyKillTracker;

    public int KilledEnemies => enemyKillTracker.killedEnemies;
    public float BuffDamage => enemyKillTracker.killedEnemies * Stats.damageBuffPerKill;

    private void Awake()
    {
        Stats = Instantiate(defaultStats);
        enemyKillTracker.OnUpdated += RaiseOnAbilityUpdated;
    }

    private void OnDestroy()
    {
        enemyKillTracker.OnUpdated -= RaiseOnAbilityUpdated;
    }

    public override bool IsPassive()
    {
        return true;
    }

    public override bool CanDeploy()
    {
        return true;
    }
}
