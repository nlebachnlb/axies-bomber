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

    public List<StatsBuff> buffs { get; private set; } = new List<StatsBuff>();
    public UpgradeBuff upgradeBuff { get; private set; } = new();

    public float GetBaseValue(Stat stat)
    {
        float baseValue = 0;
        switch (stat)
        {
            case Stat.Speed:
                baseValue = speed;
                break;
            case Stat.BombMagazine:
                baseValue = bombMagazine;
                break;
            case Stat.Health:
                baseValue = health;
                break;
            case Stat.BombExplosionRadius:
                baseValue = bombExplosionRadius;
                break;
        }
        return baseValue;
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

    public void ResetBuffs()
    {
        buffs.Clear();
    }

    public AxieStats Calculate()
    {
        AxieStats result = Instantiate(this);
        //Debug.Log("Before: " + result);
        foreach (StatsBuff buff in buffs)
        {
            switch (buff.buffType)
            {
                case Stat.Speed:
                    //Debug.Log("Speed buff: " + result.speed + " + " + buff.GetDeltaValueFromBase(speed));
                    result.speed += buff.GetDeltaValueFromBase(speed);
                    break;
                case Stat.BombMagazine:
                    //Debug.Log("Bomb buff: " + result.bombMagazine + " + " + buff.GetDeltaValueFromBase(bombMagazine));
                    result.bombMagazine += (int)buff.GetDeltaValueFromBase(bombMagazine);
                    break;
                case Stat.Health:
                    result.health += (int)buff.GetDeltaValueFromBase(health);
                    break;
                case Stat.BombExplosionRadius:
                    result.bombExplosionRadius += (int)buff.GetDeltaValueFromBase(bombExplosionRadius);
                    break;
            }
        }
        result.speed += upgradeBuff.speed;
        result.bombExplosionRadius += upgradeBuff.bombExplosionRadius;
        result.bombMagazine += upgradeBuff.bombMagazine;
        result.health += upgradeBuff.health;
        //Debug.Log("After: " + result);
        return result;
    }

    public void AddUpgradeBuff(UpgradeBuff upgradeBuff)
    {
        this.upgradeBuff = upgradeBuff;
    }

    public override string ToString()
    {
        string result = "{speed: " + speed + ", bombMagazine: " + bombMagazine + ", health: " + health + ", radius: " + bombExplosionRadius + "}";
        return result;
    }
}