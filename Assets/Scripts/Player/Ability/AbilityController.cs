using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    public AxieAbility Ability { get; set; }
    public KeyCode deployKey;
    public GameObject owner;

    private void Awake()
    {
        Ability = GetComponentInChildren<AxieAbility>();
        Ability.Owner = owner;
    }

    private void Update()
    {
        if (Input.GetKeyDown(deployKey) && Ability.CanDeploy())
        {
            Debug.Log("Active ability");
            Ability.DeployAbility();
        }
    }
}
