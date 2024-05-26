using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[System.Serializable]
public class AxieHeroData
{
    public struct InfoPacket
    {
        public int health;
        public int bombsRemaining;
        public int healthLimit;
        public int bombMagazine;
    }

    [Header("Base stats & config")]
    public AxieIdentity identity;
    public AxieConfig axieConfig;
    public AxieStats axieStats;
    public BombStats bombStats;

    [Obsolete]
    public SkillConfig ability;
    [Obsolete]
    public AxieAbility abilityPrefab;
    public Dictionary<string, float> extraParams = new Dictionary<string, float>();

    public Dictionary<SkillType, AxieAbility> abilityPrefabs = new();
    public Dictionary<SkillType, AxieAbility> abilityInstances = new();

    public int health
    {
        get => _health;
        set
        {
            if (value != _health)
            {
                _health = Mathf.Clamp(value, 0, axieStats.Calculate().health);
                onInfoChanged?.Invoke(GetCurrentInfo());
            }
        }
    }

    public int bombsRemaining
    {
        get => _bombsRemaining;
        set
        {
            if (value != _bombsRemaining)
            {
                _bombsRemaining = Mathf.Clamp(value, 0, axieStats.Calculate().bombMagazine);
                onInfoChanged?.Invoke(GetCurrentInfo());
            }
        }
    }

    public float bombDamage
    {
        get
        {
            var buffPercentage = 0f;
            foreach (var item in abilityInstances)
            {
                if (item.Value is IBuff buff)
                {
                    buffPercentage += buff.BuffDamage;
                }
            }

            Debug.Log($"Bomb damage increase by {buffPercentage * 100f}% by skill");
            return bombStats.Calculate().damage * (1 + buffPercentage);
        }
    }

    private int _health;
    private int _bombsRemaining;

    public delegate void OnInfoChanged(InfoPacket packet);
    public event OnInfoChanged onInfoChanged;

    public bool IsDead { get => _health <= 0; }

    public void ReloadInGameData()
    {
        AxieStats calculated = axieStats.Calculate();
        health = calculated.health;
        bombsRemaining = calculated.bombMagazine;
    }

    public void RaiseUpdateInfo()
    {
        onInfoChanged?.Invoke(GetCurrentInfo());
    }

    private InfoPacket GetCurrentInfo()
    {
        AxieStats calculated = axieStats.Calculate();
        InfoPacket info = new InfoPacket();
        info.health = health;
        info.bombsRemaining = bombsRemaining;
        info.healthLimit = calculated.health;
        info.bombMagazine = calculated.bombMagazine;
        return info;
    }

    #region Extra params

    public const string PARAM_KILLED_ENEMIES = "killedEnemies";
    public const string PARAM_PLACED_BOMBS = "placedBombs";
    public const string PARAM_COOLDOWN_TIME = "cooldownTime";
    public const string PARAM_SPEED_MULTIPLIER = "speedMultiplier";

    public float GetExtraParam(string key, float defaultValue = 0)
    {
        if (extraParams.ContainsKey(key))
            return extraParams[key];
        extraParams.Add(key, defaultValue);
        return defaultValue;
    }

    public void SetExtraParam(string key, float value)
    {
        if (extraParams.ContainsKey(key))
            extraParams[key] = value;
        else
            extraParams.Add(key, value);
    }

    #endregion
}
