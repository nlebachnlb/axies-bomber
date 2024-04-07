using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SkillConfigWithLevel
{
    public List<SkillConfig> skills;
}

[System.Serializable]
public class AxiePackedConfig
{
    public int id;
    public AxieConfig axieConfig;
    public AxieStats axieStats;
    public BombStats bombStats;
    public List<SkillConfigWithLevel> skillConfigs;
    public List<SkillConfig> ability;
    public AxieAbility abilityPrefab;
}
