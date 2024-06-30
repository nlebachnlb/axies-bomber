using Ability.Component;

public class NaturalHealingUI : AbilityUI<NaturalHealing>
{
    private Cooldown cooldown;

    public override void Init(AbilitySlot slot, NaturalHealing ability)
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
