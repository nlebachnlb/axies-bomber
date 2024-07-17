using Ability.Component;

public class JuicyRushUI : AbilityUI<JuicyRush>
{
    private Timer timer;

    public override void Init(AbilitySlot slot, JuicyRush ability)
    {
        base.Init(slot, ability);

        timer = ability.Timer;
        slot.SetCardChargedState(ability.CanDeploy());
    }

    public override void OnDispose()
    {
        slot.SetCardChargedState(false);
    }

    private void Update()
    {
        slot.SetCardChargedState(ability.CanDeploy());
        slot.SetCountdownState(!timer.IsAvailable, timer.RemainingTime, timer.RemainingTimeAsPercentage);
    }
}
