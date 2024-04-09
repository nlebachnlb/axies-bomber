using System;
using System.Collections;
using System.Collections.Generic;
using ExcelConfig;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AxieUpgradeConfig", menuName = "Config/AxieUpgradeConfig")]
public class AxieUpgradeConfig : SerializedScriptableObject
{
    [HideReferenceObjectPicker]
    public class DataPacket
    {
        public float UpgradeValue;
        public int UpgradeCost;
    }

    [Serializable, HideReferenceObjectPicker]
    public class StatUpgradeData
    {
        [TableList(ShowIndexLabels = true, AlwaysExpanded = true, ShowPaging = true)]
        public List<DataPacket> Data = new();

        public int MaxLevel => Data.Count - 1;
    }

    [SerializeField]
    [DictionaryDrawerSettings(KeyLabel = "Axie", ValueLabel = "Config")]
    private Dictionary<AxieIdentity, Dictionary<Stat, StatUpgradeData>> data = new();

    public int GetStatMaxLevel(AxieIdentity id, Stat stat) => data[id][stat].MaxLevel;

    public DataPacket GetDataPacket(AxieIdentity axie, Stat stat, int level)
    {
        if (!data.TryGetValue(axie, out var statUpgradeDictionary))
            return null;

        if (!statUpgradeDictionary.TryGetValue(stat, out var config))
            return null;

        return config.Data[level];
    }

    public float GetSpeedUpgrade(AxieIdentity axie, int level)
    {
        return GetUpgradeValue(axie, Stat.Speed, level);
    }

    public int GetHealthUpgrade(AxieIdentity axie, int level)
    {
        return Mathf.RoundToInt(GetUpgradeValue(axie, Stat.Health, level));
    }

    public int GetBombExplosionRadiusUpgrade(AxieIdentity axie, int level)
    {
        return Mathf.RoundToInt(GetUpgradeValue(axie, Stat.BombExplosionRadius, level));
    }

    public int GetBombMagazineUpgrade(AxieIdentity axie, int level)
    {
        return Mathf.RoundToInt(GetUpgradeValue(axie, Stat.BombMagazine, level));
    }

    public float GetUpgradeValue(AxieIdentity axie, Stat stat, int level)
    {
        DataPacket dataPacket = GetDataPacket(axie, stat, level);
        return dataPacket != null ? dataPacket.UpgradeValue : 0;
    }

    public void Import(AxieUpgradeSheet axieUpgradeSheet)
    {
#if UNITY_EDITOR
        data?.Clear();
        data ??= new();
        TransformData(axieUpgradeSheet);
        EditorUtility.SetDirty(this);
#endif
    }

#if UNITY_EDITOR
    private void TransformData(AxieUpgradeSheet axieUpgradeSheet)
    {
        foreach (var item in axieUpgradeSheet)
        {
            var axie = item.Id;
            var dict = new Dictionary<Stat, StatUpgradeData>();

            var speeds = new List<DataPacket>();
            var healths = new List<DataPacket>();
            var bombRanges = new List<DataPacket>();
            var bombCounts = new List<DataPacket>();

            foreach (var upgrade in item.Arr)
            {
                speeds.Add(new DataPacket() { UpgradeValue = upgrade.Speed, UpgradeCost = upgrade.SpeedCost });
                healths.Add(new DataPacket() { UpgradeValue = upgrade.Health, UpgradeCost = upgrade.HealthCost });
                bombRanges.Add(new DataPacket() { UpgradeValue = upgrade.BombRange, UpgradeCost = upgrade.BombRangeCost });
                bombCounts.Add(new DataPacket() { UpgradeValue = upgrade.BombCount, UpgradeCost = upgrade.BombCountCost });
            }

            dict.TryAdd(Stat.Speed, new() { Data = speeds });
            dict.TryAdd(Stat.Health, new() { Data = healths });
            dict.TryAdd(Stat.BombExplosionRadius, new() { Data = bombRanges });
            dict.TryAdd(Stat.BombMagazine, new() { Data = bombCounts });

            data[axie] = dict;
        }
    }
#endif
}
