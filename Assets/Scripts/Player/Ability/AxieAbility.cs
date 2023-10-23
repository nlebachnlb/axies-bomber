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

    public GameObject Owner { get; set; }
}

public class AxieAbility<T> : AxieAbility
    where T : SkillConfig
{
    public T Stats { get; set; }
}