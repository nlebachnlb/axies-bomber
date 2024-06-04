using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[Serializable]
public class SkillConfigWithLevel
{
    public List<SkillConfig> skills;
}

[Serializable, HideReferenceObjectPicker]
public class AxiePackedConfig
{
    public int id;
    public AxieConfig axieConfig;
    public AxieStats axieStats;
    public BombStats bombStats;
    public AxieAbility abilityPrefab;
    public Dictionary<SkillType, AxieAbility> abilities;

    [Obsolete]
    public List<SkillConfigWithLevel> skillConfigs;
    [Obsolete]
    public List<SkillConfig> ability;
}
