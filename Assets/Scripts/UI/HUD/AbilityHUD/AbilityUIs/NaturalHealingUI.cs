using Ability.Component;

public class NaturalHealingUI : AbilityUI<NaturalHealing>
{
    private Cooldown cooldown;

    public override void Init(AbilitySlot slot, NaturalHealing ability)
    {
        base.Init(slot, ability);

        cooldown = ability.Cooldown;
        slot.SetCardChargedState(ability.CanDeploy());
    }

    public override void OnDispose()
    {
        slot.SetCardChargedState(false);
    }

    private void Update()
    {
        slot.SetCardChargedState(ability.CanDeploy());
        slot.SetCountdownState(!cooldown.IsAvailable, cooldown.RemainingTime, cooldown.RemainingTimeAsPercentage);
    }
}
