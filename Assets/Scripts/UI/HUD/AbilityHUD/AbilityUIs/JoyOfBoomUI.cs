public class JoyOfBoomUI : AbilityUI<JoyOfBoom>
{
    public override void Init(AbilitySlot slot, JoyOfBoom ability)
    {
        base.Init(slot, ability);

        slot.SetCardChargedState(true);
        slot.textAuxilliary.gameObject.SetActive(true);
        SetText();
        ability.OnAbilityUpdated += OnAbilityUpdated;
    }

    public override void OnDispose()
    {
        slot.SetCardChargedState(false);
        slot.textAuxilliary.gameObject.SetActive(false);
        ability.OnAbilityUpdated -= OnAbilityUpdated;
    }

    private void OnAbilityUpdated() => SetText();

    private void SetText() => slot.textAuxilliary.text = ability.KilledEnemies.ToString();
}
