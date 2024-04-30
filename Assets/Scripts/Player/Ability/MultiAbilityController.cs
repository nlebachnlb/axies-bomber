using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MultiAbilityController : SerializedMonoBehaviour
{
    public GameObject owner;

    [ShowInInspector, HideInEditorMode]
    private Dictionary<SkillType, KeyCode> skillDeploymentKeys;

    [ShowInInspector, HideInEditorMode]
    public Dictionary<SkillType, AxieAbility> Abilities { get; private set; } = new();

    private void Awake()
    {
        skillDeploymentKeys = AppRoot.Instance.Config.inputSettings.skillDeploymentKeys;
        EventBus.onSwitchAxieHero += OnSwitchHero;
    }

    private void OnDestroy()
    {
        EventBus.onSwitchAxieHero -= OnSwitchHero;
    }

    private void Update()
    {
        if (Abilities == null)
            return;

        foreach (var item in Abilities)
        {
            var skillType = item.Key;
            var ability = item.Value;

            if (ability == null || !skillDeploymentKeys.TryGetValue(skillType, out var deployKey))
                continue;

            if (Input.GetKeyDown(deployKey))
                ability.DeployAbility();
        }
    }

    private AxieAbility AttachAbility(SkillType skillType, AxieAbility axieAbility)
    {
        var ability = Instantiate(axieAbility, transform);
        Abilities[skillType] = ability;
        ability.Owner = owner;
        EventBus.RaiseOnAbilityAttached(skillType, ability);
        return ability;
    }

    private void DetachAllAbilities()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
        Abilities.Clear();
    }

    private void OnSwitchHero(AxieHeroData axieHeroData)
    {
        if (axieHeroData == null && axieHeroData.abilities == null)
            return;

        DetachAllAbilities();
        foreach(var item in axieHeroData.abilities)
        {
            AttachAbility(item.Key, item.Value).SetExtraParams(axieHeroData);
        }
    }
}
