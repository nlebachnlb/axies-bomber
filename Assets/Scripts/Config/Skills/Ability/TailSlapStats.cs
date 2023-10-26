using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TailSlapStats", menuName = "Stats/Ability/Tail Slap")]
public class TailSlapStats : SkillConfig
{
    public int placedBombsNeeded;
    public float speed = 5f;

    public override string GenerateDescription(int level = 0)
    {
        return description;
    }

    public override string GenerateMinorDescription(int level = 0)
    {
        return minorDescription.Replace("{placedBombsNeeded}", "" + placedBombsNeeded);
    }
}
