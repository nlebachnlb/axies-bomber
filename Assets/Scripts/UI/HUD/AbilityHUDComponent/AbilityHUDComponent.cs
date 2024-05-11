using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHUDComponent : MonoBehaviour
{
    protected AbilityHUDv2 hud;

    protected virtual void Awake()
    {
        hud = GetComponent<AbilityHUDv2>();
    }
}
