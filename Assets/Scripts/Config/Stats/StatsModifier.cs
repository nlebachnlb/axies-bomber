using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsModifier : MonoBehaviour
{
    [Header("Base stats")]
    public AxieStats axieStats;
    public BombStats bombStats;

    public void ModifyAxieStats(AxieStats modifier)
    {
        axieStats.health += modifier.health;
        axieStats.speed += modifier.speed;
        axieStats.bombExplosionRadius += modifier.bombExplosionRadius;
        axieStats.bombMagazine += modifier.bombMagazine;
    }

    public void ModifyBombStats(BombStats modifier)
    {
        bombStats.length += modifier.length;
        bombStats.explosionDuration += modifier.explosionDuration;
        bombStats.bombFuseTime += modifier.bombFuseTime;
    }
}
