using UnityEngine;

[CreateAssetMenu(fileName = "TinyCatapultStats", menuName = "Stats/Ability/Tiny Catapult")]
public class TinyCatapultStats : SkillConfig
{
    public float cooldownTime;
    public float sturdyDuration;

    public override string GenerateDescription()
    {
        return description.Replace("{sturdyDuration}", "" + sturdyDuration);
    }

    public override string GenerateMinorDescription()
    {
        return minorDescription.Replace("{cooldownTime}", "" + cooldownTime);
    }
}
