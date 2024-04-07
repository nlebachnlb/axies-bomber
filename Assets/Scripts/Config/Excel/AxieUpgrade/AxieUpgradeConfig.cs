using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using ExcelConfig;
using UnityEngine;

[CreateAssetMenu(fileName = "AxieUpgradeConfig", menuName = "Config/AxieUpgradeConfig")]
public class AxieUpgradeConfig : ScriptableObject
{
    public class DataPacket
    {
        public float UpgradeValue;
        public int UpgradeCost;
    }

    public SerializedDictionary<AxieIdentity, List<AxieUpgradeConfigData>> Data = new();

    private Dictionary<AxieIdentity, Dictionary<Stat, List<DataPacket>>> mapping { get; set; }

    private void BuildMapping()
    {
        mapping = new();
        foreach (var item in Data)
        {
            var dict = new Dictionary<Stat, List<DataPacket>>();
            var s = new List<DataPacket>();
            var h = new List<DataPacket>();
            var br = new List<DataPacket>();
            var bc = new List<DataPacket>();

            foreach (var upgrade in item.Value)
            {
                s.Add(new DataPacket(){ UpgradeValue = upgrade.Speed, UpgradeCost = upgrade.SpeedCost });
                h.Add(new DataPacket(){ UpgradeValue = upgrade.Health, UpgradeCost = upgrade.HealthCost });
                br.Add(new DataPacket(){ UpgradeValue = upgrade.BombRange, UpgradeCost = upgrade.BombRangeCost });
                bc.Add(new DataPacket(){ UpgradeValue = upgrade.BombCount, UpgradeCost = upgrade.BombCountCost });
            }

            dict.TryAdd(Stat.Speed, s);
            dict.TryAdd(Stat.Health, h);
            dict.TryAdd(Stat.BombExplosionRadius, br);
            dict.TryAdd(Stat.BombMagazine, bc);

            mapping[item.Key] = dict;
        }
    }

    public DataPacket GetDataPacket(AxieIdentity axie, Stat stat, int level)
    {
        if (mapping == null || mapping.Count == 0)
            BuildMapping();

        if (!mapping.TryGetValue(axie, out var dict))
            return null;

        if (!dict.TryGetValue(stat, out var upgradeConfig))
            return null;

        return upgradeConfig[level];
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
}
