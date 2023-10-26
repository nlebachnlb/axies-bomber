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


    private static EventBus instance = null;
}
