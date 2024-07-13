using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestHeroUI : AbilityUI<ForestHero>
{
    public override void Init(AbilitySlot slot, ForestHero ability)
    {
        base.Init(slot, ability);
        slot.SetCardChargedState(true);
    }

    public override void OnDispose()
    {
        slot.SetCardChargedState(false);
    }
}
