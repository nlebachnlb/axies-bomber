using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SucculentBloomUI : AbilityUI<SucculentBloom>
{
    public override void Init(AbilitySlot slot, SucculentBloom ability)
    {
        base.Init(slot, ability);
        slot.SetCardChargedState(true);
    }

    public override void OnDispose()
    {
        slot.SetCardChargedState(false);
    }
}
