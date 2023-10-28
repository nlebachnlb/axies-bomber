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

    public delegate void OnBombPlace(AxieHeroData owner);
    public static event OnBombPlace onBombPlace;

    public delegate void OnSwitchAxieHero(AxieHeroData axieHeroData);
    public static event OnSwitchAxieHero onSwitchAxieHero;

    public delegate void OnAxieHeroDeath(AxieHeroData axieHeroData);
    public static event OnAxieHeroDeath onAxieHeroDeath;

    public delegate void OnPickAxie(int slot, AxiePackedConfig config);
    public static event OnPickAxie onPickAxie;

    public delegate void OnOpenSkillPool(bool isAbilityPool);
    public static event OnOpenSkillPool onOpenSkillPool;

    public delegate void OnEnterSkillPool(List<SkillConfig> skills);
    public static event OnEnterSkillPool onEnterSkillPool;

    public delegate void OnPickSkill(SkillConfig skill);
    public static event OnPickSkill onPickSkill;

    public delegate void OnMapChange(string newId);
    public static event OnMapChange onMapChange;

    public delegate void OnRoomClear();
    public static event OnRoomClear onRoomClear;

    public delegate void OnEnemyDeath();
    public static event OnEnemyDeath onEnemyDeath;

    public delegate void OnAbilityCooldown(float current, float max, int displayType);
    public static event OnAbilityCooldown onAbilityCooldown;

    public delegate void OnGameOver();
    public static event OnGameOver onGameOver;


    public static void RaiseOnBombFuse()
    {
        onBombFuse?.Invoke();
    }

    public static void RaiseOnBombPlace(AxieHeroData owner)
    {
        onBombPlace?.Invoke(owner);
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

    public static void RaiseOnOpenSkillPool(bool isAbilityPool)
    {
        onOpenSkillPool?.Invoke(isAbilityPool);
    }

    public static void RaiseOnMapChange(string newId)
    {
        onMapChange?.Invoke(newId);
    }

    public static void RaiseOnRoomClear()
    {
        onRoomClear?.Invoke();
    }

    public static void RaiseOnEnemyDeath()
    {
        onEnemyDeath?.Invoke();
    }

    public static void RaiseOnAbilityCooldown(float current, float max, int displayType)
    {
        onAbilityCooldown?.Invoke(current, max, displayType);
    }

    public static void RaiseOnGameOver()
    {
        onGameOver?.Invoke();
    }

    private static EventBus instance = null;
}
