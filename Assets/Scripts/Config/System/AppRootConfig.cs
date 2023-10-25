using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AppRoot config", menuName = "Config/System/AppRoot")]
public class AppRootConfig : ScriptableObject
{
    public string startSceneName;
    public string playScene;
    public string homeScene;
    public AvailableAxieHeroesConfig availableAxies;

    [System.Serializable]
    public class MapConfig
    {
        public string id;
        public GameObject tilemap;
    }

    [SerializeField] private MapConfig[] mapConfigs;
    private Dictionary<string, MapConfig> indexedMapConfigs;

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
