using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxieHeroData
{
    [Header("Base stats & config")]
    public AxieConfig axieConfig;
    public AxieStats axieStats;
    public BombStats bombStats;

    [Header("In game data")]
    public int health;
    public int bombsRemaining;

    public void ReloadInGameData()
    {
        health = axieStats.health;
        bombsRemaining = axieStats.bombMagazine;
    }
}
