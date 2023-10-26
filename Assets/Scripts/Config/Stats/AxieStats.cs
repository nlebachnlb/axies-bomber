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
                case StatsBuff.BuffType.Speed:
                    //Debug.Log("Speed buff: " + result.speed + " + " + buff.GetDeltaValueFromBase(speed));
                    result.speed += buff.GetDeltaValueFromBase(speed);
                    break;
                case StatsBuff.BuffType.BombMagazine:
                    //Debug.Log("Bomb buff: " + result.bombMagazine + " + " + buff.GetDeltaValueFromBase(bombMagazine));
                    result.bombMagazine += (int)buff.GetDeltaValueFromBase(bombMagazine);
                    break;
                case StatsBuff.BuffType.Health:
                    result.health += (int)buff.GetDeltaValueFromBase(health);
                    break;
                case StatsBuff.BuffType.BombExplosionRadius:
                    result.bombExplosionRadius += (int)buff.GetDeltaValueFromBase(bombExplosionRadius);
                    break;
            }
        }
        //Debug.Log("After: " + result);
        return result;
    }

    public override string ToString()
    {
        string result = "{speed: " + speed + ", bombMagazine: " + bombMagazine + ", health: " + health + ", radius: " + bombExplosionRadius + "}";
        return result;
    }
}