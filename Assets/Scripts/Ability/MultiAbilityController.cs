using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MultiAbilityController : MonoBehaviour
{
    [SerializeField] private GameObject owner;

    private AxieHeroData activeAxie;
    private Dictionary<AxieIdentity, AbilityController> abilityControllers = new();

    public AbilityController ActiveController => abilityControllers[activeAxie.identity];

    private void Awake()
    {
        EventBus.onSwitchAxieHero += OnSwitchHero;
    }

    private void OnDestroy()
    {
        EventBus.onSwitchAxieHero -= OnSwitchHero;
    }

    private void OnSwitchHero(AxieHeroData axieHeroData)
    {
        if (activeAxie != null && abilityControllers.TryGetValue(activeAxie.identity, out var currentController))
        {
            currentController.gameObject.SetActive(false);
        }

        if (abilityControllers.TryGetValue(axieHeroData.identity, out var controller))
        {
            controller.gameObject.SetActive(true);
        }
        else
        {
            var newController = new GameObject($"AbilityController (axie {axieHeroData.identity})").AddComponent<AbilityController>();
            newController.Init(axieHeroData, owner);
            newController.transform.SetParent(transform);
            newController.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            abilityControllers[axieHeroData.identity] = newController;
        }
        activeAxie = axieHeroData;
    }
}
