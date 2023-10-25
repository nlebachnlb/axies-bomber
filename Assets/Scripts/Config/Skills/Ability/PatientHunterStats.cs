using UnityEngine;

[CreateAssetMenu(fileName = "PatientHunterStats", menuName = "Stats/Ability/Patient Hunter")]
public class PatientHunterStats : SkillConfig
{
    public int killsNeeded;
    public float dismissDuration;

    public override string GenerateDescription(int level = 0)
    {
        return description.Replace("{dismissDuration}", "" + dismissDuration);
    }

    public override string GenerateMinorDescription(int level = 0)
    {
        return minorDescription.Replace("{killsNeeded}", "" + killsNeeded);
    }
}
