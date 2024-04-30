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
        EventBus.onAbilityAttached += OnAbilityAttached;
    }

    private void OnDestroy()
    {
        EventBus.onAbilityAttached -= OnAbilityAttached;
    }

    private void OnAbilityAttached(SkillType skillType, AxieAbility axieAbility)
    {
        if (huds.TryGetValue(skillType, out var hud))
            hud.Assign(skillType, axieAbility);
    }
}
