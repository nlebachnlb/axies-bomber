using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Stats Buff", menuName = "Stats/Buff")]
public class StatsBuff : SkillConfig
{
    public enum BuffType
    {
        Speed,
        BombMagazine,
        Health,
        BombExplosionRadius
    }

    public enum BuffUnit
    {
        Unit,
        Percentage
    }

    public BuffType buffType;
    public BuffUnit buffUnit;
    public List<float> buffValue;

    public float GetValueFromBase(float baseValue, int level = 0)
    {
        if (buffUnit == BuffUnit.Percentage)
            return baseValue * (1f + (buffValue[level] / 100f));

        if (buffUnit == BuffUnit.Unit)
            return baseValue + buffValue[level];

        return baseValue;
    }

    public override string GenerateDescription(int level = 0)
    {
        return description.Replace("{value}", "" + buffValue[level]);
    }

    public override string GenerateMinorDescription(int level = 0)
    {
        return "";
    }
}
