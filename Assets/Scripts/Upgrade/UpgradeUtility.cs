using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UpgradeUtility
{
    public static float GetBaseValue(AxiePackedConfig axiePackedConfig, Stat stat)
    {
        float baseValue = 0;
        AxieStats axieStats = axiePackedConfig.axieStats;
        switch (stat)
        {
            case Stat.Speed:
                baseValue = axieStats.speed;
                break;
            case Stat.BombMagazine:
                baseValue = axieStats.bombMagazine;
                break;
            case Stat.Health:
                baseValue = axieStats.health;
                break;
            case Stat.BombExplosionRadius:
                baseValue = axieStats.bombExplosionRadius;
                break;
        }
        return baseValue;
    }
}
