using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Stats Buff", menuName = "Stats/Buff")]
public class StatsBuff : SkillConfig
{
    public enum BuffUnit
    {
        Unit,
        Percentage
    }

    public Stat buffType;
    public BuffUnit buffUnit;
    public List<float> buffValue;

    public float GetValueFromBase(float baseValue)
    {
        if (buffUnit == BuffUnit.Percentage)
            return baseValue * (1f + (buffValue[level] / 100f));

        if (buffUnit == BuffUnit.Unit)
            return baseValue + buffValue[level];

        return baseValue;
    }

    public float GetDeltaValueFromBase(float baseValue)
    {
        if (buffUnit == BuffUnit.Percentage)
            return baseValue * (buffValue[level] / 100f);

        if (buffUnit == BuffUnit.Unit)
            return buffValue[level];

        return 0f;
    }

    public override string GenerateDescription()
    {
        return description.Replace("{value}", "" + buffValue[level]);
    }

    public override string GenerateMinorDescription()
    {
        return "";
    }
}
