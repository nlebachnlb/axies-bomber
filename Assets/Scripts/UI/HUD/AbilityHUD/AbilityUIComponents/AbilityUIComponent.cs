using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ability.UI
{
    public class AbilityUIComponent : MonoBehaviour
    {
        protected AbilityHUD hud;

        protected virtual void Awake()
        {
            hud = GetComponent<AbilityHUD>();
        }
    }
}