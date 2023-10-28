using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus
{
    public static EventBus Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventBus();
            }

            return instance;
        }
    }

    public delegate void OnBombFuse();
    public static event OnBombFuse onBombFuse;

    public delegate void OnSwitchAxieHero(AxieHeroData axieHeroData);
    public static event OnSwitchAxieHero onSwitchAxieHero;

    public delegate void OnAxieHeroDeath(AxieHeroData axieHeroData);
    public static event OnAxieHeroDeath onAxieHeroDeath;

    public delegate void OnPickAxie(int slot, AxiePackedConfig config);
    public static event OnPickAxie onPickAxie;

    public delegate void OnOpenSkillPool();
    public static event OnOpenSkillPool onOpenSkillPool;

    public delegate void OnEnterSkillPool(List<SkillConfig> skills);
    public static event OnEnterSkillPool onEnterSkillPool;

    public delegate void OnPickSkill(SkillConfig skill);
    public static event OnPickSkill onPickSkill;

    public delegate void OnMapChange(string newId);
    public static event OnMapChange onMapChange;

    public delegate void OnEnemyDeath();
    public static event OnEnemyDeath onEnemyDeath;


    public static void RaiseOnBombFuse()
    {
        onBombFuse?.Invoke();
    }

    public static void RaiseOnSwitchAxieHero(AxieHeroData axieHeroData)
    {
        onSwitchAxieHero?.Invoke(axieHeroData);
    }

    public static void RaiseOnAxieHeroDeath(AxieHeroData axieHeroData)
    {
        onAxieHeroDeath?.Invoke(axieHeroData);
    }

    public static void RaiseOnPickAxie(int slot, AxiePackedConfig config)
    {
        onPickAxie?.Invoke(slot, config);
    }

    public static void RaiseOnEnterSkillPool(List<SkillConfig> skills)
    {
        onEnterSkillPool?.Invoke(skills);
    }

    public static void RaiseOnPickSkill(SkillConfig skill)
    {
        onPickSkill?.Invoke(skill);
    }

    public static void RaiseOnOpenSkillPool()
    {
        onOpenSkillPool?.Invoke();
    }

    public static void RaiseOnMapChange(string newId)
    {
        onMapChange?.Invoke(newId);
    }

    public static void RaiseOnEnemyDeath()
    {
        onEnemyDeath?.Invoke();
    }

    private static EventBus instance = null;
}
