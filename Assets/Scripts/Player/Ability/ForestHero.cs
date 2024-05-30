using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestHero : AxieAbility<ForestHeroStats>
{
    [SerializeField] private ForestHeroStats defaultStat;

    public float CriticalThreshold => Stats.criticalThreshold;

    private void Awake()
    {
        Stats = Instantiate(defaultStat);
    }
}
