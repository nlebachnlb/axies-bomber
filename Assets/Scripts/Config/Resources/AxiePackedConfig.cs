using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SkillConfigWithLevel
{
    public List<SkillConfig> skills;
}

public enum AxieIdentity
{
    Aquatic = 0,
    Bird = 1,
    Reptile = 2
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
}
