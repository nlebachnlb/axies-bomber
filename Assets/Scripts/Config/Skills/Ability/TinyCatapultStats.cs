using UnityEngine;

[CreateAssetMenu(fileName = "TinyCatapultStats", menuName = "Stats/Ability/Tiny Catapult")]
public class TinyCatapultStats : SkillConfig
{
    public float cooldownTime;
    public float sturdyDuration;

    public override string GenerateDescription(int level = 0)
    {
        return description.Replace("{sturdyDuration}", "" + sturdyDuration);
    }

    public override string GenerateMinorDescription(int level = 0)
    {
        return minorDescription.Replace("{cooldownTime}", "" + cooldownTime);
    }
}
