using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// One AbilityController per axie
/// </summary>
public class AbilityController : MonoBehaviour
{
    public Dictionary<SkillType, AxieAbility> Abilities { get; private set; } = new();
    public GameObject owner { get; private set; }
    private Dictionary<SkillType, KeyCode> skillDeploymentKeys;

    public void Init(AxieHeroData axieHeroData, GameObject owner)
    {
        this.owner = owner;
        if (axieHeroData == null || axieHeroData.abilityPrefabs == null)
            return;

        axieHeroData.abilityInstances ??= new();
        foreach (var item in axieHeroData.abilityPrefabs)
        {
            var abilityInstance = AttachAbility(item.Key, item.Value);
            abilityInstance.SetExtraParams(axieHeroData);
            axieHeroData.abilityInstances[item.Key] = abilityInstance;
        }
    }

    private void Awake()
    {
        skillDeploymentKeys = AppRoot.Instance.Config.inputSettings.skillDeploymentKeys;
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

            if (ability.IsPassive() || (Input.GetKeyDown(deployKey) && ability.CanDeploy()))
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
}
