using System.Collections;
using System.Collections.Generic;
using Ability.Component;
using UnityEngine;

public class SneakySketchUI : AbilityUI<SneakySketch>
{
    private Cooldown cooldown;

    public override void Init(AbilitySlot slot, SneakySketch ability)
    {
        base.Init(slot, ability);

        cooldown = ability.Cooldown;
        slot.SetActivateState(ability.CanDeploy());
    }

    public override void OnDispose()
    {
        slot.SetActivateState(false);
    }

    private void Update()
    {
        slot.SetActivateState(ability.CanDeploy());
        slot.SetCountdownState(!cooldown.IsAvailable, cooldown.RemainingTime, cooldown.RemainingTimeAsPercentage);
    }
}
