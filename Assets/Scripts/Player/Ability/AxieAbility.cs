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
    public event EventBus.OnAbilityCooldown OnCooldown;

    public virtual SkillConfig.DisplayType DisplayType { get; }

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

    public void RaiseOnCooldown(float current, float max)
    {
        OnCooldown?.Invoke(current, max, 0);
    }

    public GameObject Owner { get; set; }
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