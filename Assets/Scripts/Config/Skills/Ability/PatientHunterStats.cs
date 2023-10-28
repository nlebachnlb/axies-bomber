using UnityEngine;

[CreateAssetMenu(fileName = "PatientHunterStats", menuName = "Stats/Ability/Patient Hunter")]
public class PatientHunterStats : SkillConfig
{
    public int killsNeeded;
    public float dismissDuration;

    public override string GenerateDescription()
    {
        return description.Replace("{dismissDuration}", "" + dismissDuration);
    }

    public override string GenerateMinorDescription()
    {
        return minorDescription.Replace("{killsNeeded}", "" + killsNeeded);
    }
}
