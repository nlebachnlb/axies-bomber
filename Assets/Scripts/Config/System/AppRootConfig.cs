using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AppRoot config", menuName = "Config/System/AppRoot")]
public class AppRootConfig : ScriptableObject
{
    public string startSceneName;
    public string playScene;
    public string homeScene;

    [Header("Input")]
    public InputSetting inputSettings;

    [Header("Axies")]
    public AvailableAxieHeroesConfig availableAxies;

    [System.Serializable]
    public class MapConfig
    {
        public string id;
        public GameObject tilemap;
    }

    [Header("Maps")]
    [SerializeField] private MapConfig[] mapConfigs;
    private Dictionary<string, MapConfig> indexedMapConfigs;

    [Header("Upgrades")]
    public AxieUpgradeConfig axieUpgrades;

    [Header("Collectibles")]
    public GameObject oilPrefab;
    public GameObject floatingText;

    private void GenerateIndexedMapConfig()
    {
        indexedMapConfigs = new Dictionary<string, MapConfig>();
        for (int i = 0; i < mapConfigs.Length; ++i)
        {
            MapConfig config = mapConfigs[i];
            indexedMapConfigs.Add(config.id, config);
        }
    }

    public GameObject GetMap(string id)
    {
        if (indexedMapConfigs == null)
            GenerateIndexedMapConfig();

        if (indexedMapConfigs.ContainsKey(id))
        {
            return indexedMapConfigs[id].tilemap;
        }

        return null;
    }
}
