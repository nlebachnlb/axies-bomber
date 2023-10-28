using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    public AxieAbility Ability { get; private set; }
    public KeyCode deployKey;
    public GameObject owner;

    public void AttachAbility(AxieAbility ability)
    {
        Ability = Instantiate(ability, transform);
        Ability.Owner = owner;
    }

    public void DetachAbility()
    {
        if (Ability == null) return;
        Destroy(Ability.gameObject);
    }

    private void Awake()
    {
        EventBus.onSwitchAxieHero += OnSwitchHero;
        EventBus.onPickSkill += OnPickSkill;
    }

    private void OnDestroy()
    {
        EventBus.onSwitchAxieHero -= OnSwitchHero;
        EventBus.onPickSkill -= OnPickSkill;
    }

    private void Update()
    {
        if (Ability != null && Input.GetKeyDown(deployKey) && Ability.CanDeploy())
        {
            Debug.Log("Active ability");
            Ability.DeployAbility();
        }
    }

    private void OnSwitchHero(AxieHeroData heroData)
    {
        DetachAbility();
        if (heroData.ability != null)
        {
            AttachAbility(heroData.abilityPrefab);
            Ability.SetExtraParams(heroData);
        }
    }

    private void OnPickSkill(SkillConfig skill)
    {
    }
}
