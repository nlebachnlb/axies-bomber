using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AbilityGroup : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<SkillType, AbilityHUDv2> huds;

    private void Awake()
    {
        EventBus.onPostSwitchAxieHero += OnPostSwitchAxieHero;
        EventBus.onAbilityAttached += OnAbilityAttached;
    }

    private void OnDestroy()
    {
        EventBus.onAbilityAttached -= OnAbilityAttached;
        EventBus.onPostSwitchAxieHero -= OnPostSwitchAxieHero;
    }

    private void OnPostSwitchAxieHero(AxieHeroData axieHeroData)
    {
        foreach (var item in huds)
        {
            var type = item.Key;
            var hud = item.Value;

            if (axieHeroData.abilityInstances.TryGetValue(type, out var ability))
                hud.Assign(type, ability);
            else
                hud.SetNotAvailable();
        }
    }

    private void OnAbilityAttached(SkillType skillType, AxieAbility axieAbility)
    {
        // if (huds.TryGetValue(skillType, out var hud))
        //     hud.Assign(skillType, axieAbility);
    }
}
