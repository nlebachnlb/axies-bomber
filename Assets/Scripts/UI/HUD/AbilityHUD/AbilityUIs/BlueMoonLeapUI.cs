using DG.Tweening;
using Unity.VisualScripting;

public class BlueMoonLeapUI : AbilityUI<BlueMoonLeap>
{
    private Tweener blinkTween;
    private Ability.Component.Cooldown cooldown;

    public override void Init(AbilitySlot slot, BlueMoonLeap ability)
    {
        base.Init(slot, ability);

        cooldown = ability.Cooldown;
        slot.SetActivateState(ability.CanDeploy());
    }

    public override void OnDispose()
    {
        DisableBlinking();
        slot.SetDefaultState();
    }

    private void LateUpdate()
    {
        UpdateCooldown();
        UpdateBlinking();
        UpdateState();
    }

    private void UpdateState()
    {
        slot.SetActivateState(ability.CanDeploy());
    }

    private void UpdateCooldown()
    {
        slot.SetCountdownState(!cooldown.IsAvailable, cooldown.RemainingTime, cooldown.RemainingTimeAsPercentage);
        if (!cooldown.IsAvailable)
            DisableBlinking();
    }

    private void UpdateBlinking()
    {
        if (!cooldown.IsAvailable)
            return;

        if (!ability.IsJumpable)
        {
            EnableBlinking();
        }
        else
        {
            DisableBlinking();
        }
    }

    private void EnableBlinking()
    {
        if (blinkTween == null)
        {
            blinkTween = slot.card.DOFade(0.5f, 0.25f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void DisableBlinking()
    {
        blinkTween?.Kill();
        blinkTween = null;
        slot.card.color = slot.card.color.WithAlpha(1);
    }
}
