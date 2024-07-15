using Ability.Component;

public class BlueMoonLeapUI : AbilityUI<BlueMoonLeap>
{
    private Cooldown cooldown;

    public override void Init(AbilitySlot slot, BlueMoonLeap ability)
    {
        base.Init(slot, ability);

        cooldown = ability.Cooldown;
        slot.SetCardChargedState(ability.CanDeploy());
    }

    public override void OnDispose()
    {
        SetCardAlpha(1);
        slot.SetDefaultState();
    }

    private void LateUpdate()
    {
        UpdateCooldown();
        UpdateAlpha();
        UpdateState();
    }

    private void UpdateState()
    {
        slot.SetCardChargedState(ability.CanDeploy());
    }

    private void UpdateCooldown()
    {
        slot.SetCountdownState(!cooldown.IsAvailable, cooldown.RemainingTime, cooldown.RemainingTimeAsPercentage);
        if (!cooldown.IsAvailable)
            SetCardAlpha(1);
    }

    private void UpdateAlpha()
    {
        if (!cooldown.IsAvailable)
            return;

        if (!ability.IsJumpable)
        {
            SetCardAlpha(0.5f);
        }
        else
        {
            SetCardAlpha(1);
        }
    }

    private void SetCardAlpha(float alpha)
    {
        slot.card.color = slot.card.color.WithAlpha(alpha);
    }
}
