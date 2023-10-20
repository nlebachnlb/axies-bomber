using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[System.Serializable]
public class AxieHeroData
{
    [Header("Base stats & config")]
    public AxieConfig axieConfig;
    public AxieStats axieStats;
    public BombStats bombStats;

    public int health
    {
        get => _health;
        set
        {
            if (value != _health)
            {
                _health = value;
                onInfoChanged?.Invoke(health, bombsRemaining);
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
                _bombsRemaining = value;
                onInfoChanged?.Invoke(health, bombsRemaining);
            }
        }
    }

    private int _health;
    private int _bombsRemaining;

    public delegate void OnInfoChanged(int health, int bombsRemaining);
    public event OnInfoChanged onInfoChanged;

    public void ReloadInGameData()
    {
        health = axieStats.health;
        bombsRemaining = axieStats.bombMagazine;
    }
}
