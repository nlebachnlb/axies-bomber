using Ability.Component;

public class JuicyRushUI : AbilityUI<JuicyRush>
{
    private Cooldown cooldown;

    public override void Init(AbilitySlot slot, JuicyRush ability)
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
