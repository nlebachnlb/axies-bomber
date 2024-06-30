using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SucculentBloomUI : AbilityUI<SucculentBloom>
{
    public override void Init(AbilitySlot slot, SucculentBloom ability)
    {
        base.Init(slot, ability);
        slot.SetActivateState(true);
    }

    public override void OnDispose()
    {
        slot.SetActivateState(false);
    }
}
