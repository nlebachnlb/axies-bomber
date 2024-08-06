using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tag
{
    public const string MAP_CHANGER = "MapChanger";
    public const string ENEMY = "Enemy";
    public const string SKILL_POOL = "SkillPool";
    public const string COLLECTIBLE = "Collectible";
    public const string TRANSPORT = "Transport";
    public const string KILL_FIELD = "KillField";
}

public enum AxieIdentity
{
    Aquatic = 0,
    Bird = 1,
    Reptile = 2,
    Beast = 3,
    Plant = 4,
}

public enum Stat
{
    Speed,
    BombMagazine,
    Health,
    BombExplosionRadius
}

public enum CollectibleType
{
    Oil,
    Gear,
}

public enum SkillType
{
    Mouth,
    Horn,
    Back,
    Tail
}
