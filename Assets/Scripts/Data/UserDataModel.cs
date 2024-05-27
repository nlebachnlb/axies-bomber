using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static AxieUpgradeConfig;

[Serializable]
public class UserData
{
    public List<int> ownedAxieIds = new List<int>();
    public List<int> currentPickedAxies;
    public Dictionary<int, Dictionary<Stat, int>> axieStatLevel = new();
    public int currency1;

    public int Currency1
    {
        get => currency1;
        set
        {
            currency1 = value;
            EventBus.RaiseOnCurrency1Changed(value);
        }
    }

    public UserData()
    {
        currentPickedAxies = new List<int>()
        {
            -1, -1, -1
        };
        currency1 = 50;
    }
}

public class UserDataModel : MonoBehaviour
{
    public UserData User { get; private set; }

    public UserData GenerateDefaultUserData()
    {
        UserData user = new UserData();
        user.ownedAxieIds.Add((int)AxieIdentity.Aquatic);
        user.ownedAxieIds.Add((int)AxieIdentity.Bird);
        user.ownedAxieIds.Add((int)AxieIdentity.Reptile);
        user.ownedAxieIds.Add((int)AxieIdentity.Beast);

        return user;
    }

    public void ResetPickedAxies()
    {
        for (int i = 0; i < User.currentPickedAxies.Count; ++i)
            User.currentPickedAxies[i] = -1;
    }

    public void PickAxie(int slot, int axieId)
    {
        // Cancel the slots this axie occupies
        for (int i = 0; i < User.currentPickedAxies.Count; ++i)
        {
            if (User.currentPickedAxies[i] == axieId)
            {
                User.currentPickedAxies[i] = -1;
            }
        }

        // Put Axie into new slot
        User.currentPickedAxies[slot] = axieId;
    }

    public List<SkillConfig> GetAllSkillsFromPickedAxies()
    {
        List<SkillConfig> result = new List<SkillConfig>();
        foreach (var axie in User.currentPickedAxies)
        {
            result.AddRange(AppRoot.Instance.Config.availableAxies.GetAxieSkillConfigsById(axie));
        }
        return result;
    }

    public List<List<SkillConfig>> GetPickedAxieAbilities()
    {
        List<List<SkillConfig>> result = new List<List<SkillConfig>>();
        foreach (var axie in User.currentPickedAxies)
        {
            AxiePackedConfig axieConfig = AppRoot.Instance.Config.availableAxies.GetAxiePackedConfigById(axie);
            result.Add(new List<SkillConfig>(axieConfig.ability));
        }

        return result;
    }

    public bool IsAxiePicked(int axieId)
    {
        return User.currentPickedAxies.Contains(axieId);
    }

    public Dictionary<Stat, int> GetAxieStatsLevel(int axieId)
    {
        if (User.axieStatLevel.TryGetValue(axieId, out var info))
            return info;
        else
            return null;
    }

    public int GetAxieStatLevel(int axieId, Stat stat)
    {
        var statInfo = GetAxieStatsLevel(axieId);
        if (statInfo != null && statInfo.ContainsKey(stat))
            return statInfo[stat];
        else
            return 0;
    }

    public int IncreaseStatLevel(int axieId, Stat stat)
    {
        User.axieStatLevel.TryAdd(axieId, new());
        User.axieStatLevel[axieId].TryAdd(stat, 0);
        User.axieStatLevel[axieId][stat] += 1;
        return User.axieStatLevel[axieId][stat];
    }

    public UpgradeBuff GetUpgradeBuff(int axieId)
    {
        var statLevel = GetAxieStatsLevel(axieId);
        if (statLevel == null)
            return new();

        AxieUpgradeConfig axieUpgradeConfig = AppRoot.Instance.Config.axieUpgrades;
        AxieIdentity id = (AxieIdentity)axieId;
        return new()
        {
            speed = axieUpgradeConfig.GetSpeedUpgrade(id, GetAxieStatLevel(axieId, Stat.Speed)),
            health = axieUpgradeConfig.GetHealthUpgrade(id, GetAxieStatLevel(axieId, Stat.Health)),
            bombExplosionRadius = axieUpgradeConfig.GetBombExplosionRadiusUpgrade(id, GetAxieStatLevel(axieId, Stat.BombExplosionRadius)),
            bombMagazine = axieUpgradeConfig.GetBombMagazineUpgrade(id, GetAxieStatLevel(axieId, Stat.BombMagazine))
        };
    }

    public void Collect(Collectible collectible)
    {
        if (collectible == null)
            return;

        switch (collectible.Type)
        {
            case CollectibleType.Coin:
                User.Currency1 += collectible.Amount;
                break;
        }
    }

    private void Awake()
    {
        User = GenerateDefaultUserData();

        EventBus.onPickAxie += OnPickAxie;
    }

    private void OnPickAxie(int slot, AxiePackedConfig config)
    {
        PickAxie(slot, config.id);
    }
}
