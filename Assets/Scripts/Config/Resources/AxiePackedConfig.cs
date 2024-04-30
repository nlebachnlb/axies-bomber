using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[System.Serializable]
public class SkillConfigWithLevel
{
    public List<SkillConfig> skills;
}

[System.Serializable, HideReferenceObjectPicker]
public class AxiePackedConfig
{
    public int id;
    public AxieConfig axieConfig;
    public AxieStats axieStats;
    public BombStats bombStats;
    public List<SkillConfigWithLevel> skillConfigs;
    public List<SkillConfig> ability;
    public AxieAbility abilityPrefab;
    public Dictionary<SkillType, AxieAbility> abilities;
}
