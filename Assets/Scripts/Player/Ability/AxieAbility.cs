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

    public GameObject Owner { get; set; }
    public bool IsCooldown { get; protected set; }
}

public class AxieAbility<T> : AxieAbility
    where T : SkillConfig
{
    public T Stats { get; set; }
}