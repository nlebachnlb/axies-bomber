using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAxieAbility
{
    void DeployAbility();
    bool CanDeploy();
}

public class AxieAbility : MonoBehaviour, IAxieAbility
{
    public event Action OnAbilityUpdated;
    public event EventBus.OnAbilityCooldown OnCooldown;

    public virtual SkillConfig.DisplayType DisplayType { get; }

    public bool HasEnemyKillTracker => TryGetComponent<EnemyKillTracker>(out var _);

    public virtual void AssignOwner(GameObject owner)
    {
        Owner = owner;
    }

    public virtual bool IsPassive()
    {
        return false;
    }

    public virtual bool CanDeploy()
    {
        return true;
    }

    public virtual void DeployAbility()
    {
    }

    public virtual void SetExtraParams(AxieHeroData axieHero)
    {

    }

    protected void RaiseOnAbilityUpdated()
    {
        OnAbilityUpdated?.Invoke();
    }

    protected virtual void RaiseOnCooldown(float current, float max)
    {
        OnCooldown?.Invoke(current, max, 0);
        RaiseOnAbilityUpdated();
    }

    public GameObject Owner { get; private set; }
    public bool IsCooldown { get; protected set; }
}

public class AxieAbility<T> : AxieAbility
    where T : SkillConfig
{
    public T Stats { get; set; }

    public override SkillConfig.DisplayType DisplayType
    {
        get
        {
            return Stats != null ? Stats.displayType : SkillConfig.DisplayType.CurrentOverMax;
        }
    }
}