using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NaturalHealingStats", menuName = "Stats/Ability/Natural Healing")]
public class NaturalHealingStats : SkillConfig
{
    public float Cooldown;
    public float SelfHealHp;
    public float AllyHealRate;
    public float AllyHealHp;
}
