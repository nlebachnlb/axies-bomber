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

    protected AbilityController controller;
    public GameObject Owner => controller.Owner;
    public bool IsCooldown { get; protected set; }

    public virtual SkillConfig GetStats() => null;

    public virtual void Init(AbilityController controller)
    {
        this.controller = controller;
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

}

public class AxieAbility<T> : AxieAbility where T : SkillConfig
{
    public T Stats { get; set; }

    [SerializeField] private T defaultStats;

    protected virtual void Awake()
    {
        Stats = Instantiate(defaultStats);
    }

    public override SkillConfig GetStats()
    {
        return Stats;
    }
}
