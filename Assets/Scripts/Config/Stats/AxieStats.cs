using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Axie Stat", menuName = "Stats/Axie")]
public class AxieStats : ScriptableObject
{
    [Header("Combat")]
    public int bombExplosionRadius;
    public int bombMagazine;
    public int health;
    public float speed;
    public float recoveryTimeAfterDamage = 1f;

    public List<StatsBuff> buffs { get; private set; }

    public AxieStats()
    {
        buffs = new List<StatsBuff>();
    }

    public void AddBuff(StatsBuff buff)
    {
        buffs.Add(buff);
    }

    public void RemoveBuff(StatsBuff buff)
    {
        if (buffs.Contains(buff))
            buffs.Remove(buff);
    }

    public AxieStats Calculate()
    {
        AxieStats result = new AxieStats();
        foreach (StatsBuff buff in buffs)
        {
            switch (buff.buffType)
            {
                case StatsBuff.BuffType.Speed:
                    result.speed = buff.GetValueFromBase(speed);
                    break;
                case StatsBuff.BuffType.BombMagazine:
                    result.bombMagazine = (int)buff.GetValueFromBase(bombMagazine);
                    break;
                case StatsBuff.BuffType.Health:
                    result.health = (int)buff.GetValueFromBase(health);
                    break;
                case StatsBuff.BuffType.BombExplosionRadius:
                    result.bombExplosionRadius = (int)buff.GetValueFromBase(bombExplosionRadius);
                    break;
            }
        }
        return result;
    }
}