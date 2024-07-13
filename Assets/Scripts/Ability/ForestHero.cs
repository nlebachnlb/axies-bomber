using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestHero : AxieAbility<ForestHeroStats>
{
    public float CriticalThreshold => Stats.criticalThreshold;
}
