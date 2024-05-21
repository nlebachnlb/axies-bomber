using System;
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

    public delegate void OnPostSwitchAxieHero(AxieHeroData axieHeroData);
    public static event OnPostSwitchAxieHero onPostSwitchAxieHero;

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

    public delegate void OnMapChange(int newId);
    public static event OnMapChange onMapChange;

    public delegate void OnRoomChange(int newId, Vector2Int fromDirection);
    public static event OnRoomChange onRoomChange;

    public delegate void OnRoomClear();
    public static event OnRoomClear onRoomClear;

    public delegate void OnEnemyDeath();
    public static event OnEnemyDeath onEnemyDeath;

    [Obsolete]
    public delegate void OnAbilityCooldown(float current, float max, int displayType);
    public static event OnAbilityCooldown onAbilityCooldown;

    public delegate void OnGameOver();
    public static event OnGameOver onGameOver;

    public delegate void OnAbilityAttached(SkillType skillType, AxieAbility ability);
    public static event OnAbilityAttached onAbilityAttached;

    public static event Action<int> onCurrency1Changed;
    public static event Action<Collectible> onPickCollectible;
    public delegate void OnEnterRoom(int roomId);

    public event OnEnterRoom EnterRoomEvent;

    public delegate void OnLeaveToRoom(int roomId);

    public event OnEnterRoom LeaveToRoomEvent;


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
        onPostSwitchAxieHero?.Invoke(axieHeroData);
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

    public static void RaiseOnMapChange(int newId)
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

    [Obsolete]
    public static void RaiseOnAbilityCooldown(float current, float max, int displayType)
    {
        onAbilityCooldown?.Invoke(current, max, displayType);
    }

    public static void RaiseOnGameOver()
    {
        onGameOver?.Invoke();
    }

    public static void RaiseOnCurrency1Changed(int value)
    {
        onCurrency1Changed?.Invoke(value);
    }

    public static void RaiseOnPickCollectible(Collectible collectible)
    {
        onPickCollectible?.Invoke(collectible);
    }

    public static void RaiseOnAbilityAttached(SkillType skillType, AxieAbility axieAbility)
    {
        onAbilityAttached?.Invoke(skillType, axieAbility);
    }

    public static void  RaiseOnRoomChange(int roomId, Vector2Int fromDirection)
    {
        onRoomChange?.Invoke(roomId, fromDirection);
    }

    private static EventBus instance = null;

    public void OnEnterRoomEvent(int roomId)
    {
        EnterRoomEvent?.Invoke(roomId);
    }

    public void OnLeaveToRoomEvent(int roomId)
    {
        LeaveToRoomEvent?.Invoke(roomId);
    }
}
