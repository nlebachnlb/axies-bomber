using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TailSlapStats", menuName = "Stats/Ability/Tail Slap")]
public class TailSlapStats : SkillConfig
{
    public int enemyKillNeeded;
    public float speed = 5f;

    public override string GenerateDescription()
    {
        return description;
    }

    public override string GenerateMinorDescription()
    {
        return minorDescription.Replace("{placedBombsNeeded}", "" + enemyKillNeeded);
    }
}
