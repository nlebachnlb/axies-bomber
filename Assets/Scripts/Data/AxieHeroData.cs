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
    public SkillConfig ability;
    public AxieAbility abilityPrefab;

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
}
