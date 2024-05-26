using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "New Available Axies Config", menuName = "Config/System/Available Axies Config")]
public class AvailableAxieHeroesConfig : SerializedScriptableObject
{
    [OdinSerialize, NonSerialized, ListDrawerSettings(ListElementLabelName = "id")]
    public List<AxiePackedConfig> axiePackedConfigs;

    [NonSerialized]
    private Dictionary<int, AxiePackedConfig> configMap;

    public AxiePackedConfig GetAxiePackedConfigById(int id)
    {
        if (configMap == null)
        {
            configMap = new Dictionary<int, AxiePackedConfig>();
            foreach (AxiePackedConfig config in axiePackedConfigs)
            {
                configMap.Add(config.id, config);
            }
        }

        if (configMap.ContainsKey(id))
            return configMap[id];
        return null;
    }

    public List<SkillConfig> GetAxieSkillConfigsById(int id, int level = 0)
    {
        AxiePackedConfig config = GetAxiePackedConfigById(id);
        if (level < 0 || level >= config.skillConfigs.Count)
            return new();
        return config.skillConfigs[level].skills;
    }
}
