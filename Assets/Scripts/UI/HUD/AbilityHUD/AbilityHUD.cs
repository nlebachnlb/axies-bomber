using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AbilityHUD : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<SkillType, AbilitySlot> slots;

    private void Awake()
    {
        EventBus.onPostSwitchAxieHero += OnPostSwitchAxieHero;
    }

    private void OnDestroy()
    {
        EventBus.onPostSwitchAxieHero -= OnPostSwitchAxieHero;
    }

    private void OnPostSwitchAxieHero(AxieHeroData axieHeroData)
    {
        foreach (var item in slots)
        {
            var type = item.Key;
            var slot = item.Value;

            if (axieHeroData.abilityInstances.TryGetValue(type, out var ability))
                slot.Assign(type, ability);
            else
                slot.SetNotAvailable();
        }
    }
}
