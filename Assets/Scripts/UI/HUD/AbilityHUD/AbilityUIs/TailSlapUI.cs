using UnityEngine;

public class TailSlapUI : AbilityUI<TailSlap>
{
    public override void Init(AbilitySlot slot, TailSlap ability)
    {
        base.Init(slot, ability);

        slot.SetCardChargedState(ability.CanDeploy());
        slot.textAuxilliary.gameObject.SetActive(true);

        ability.OnAbilityUpdated += OnAbilityUpdated;
        EventBus.onEnemyDeath += OnEnemyDeath;
    }

    public override void OnDispose()
    {
        slot.textAuxilliary.gameObject.SetActive(false);
        ability.OnAbilityUpdated -= OnAbilityUpdated;
        EventBus.onEnemyDeath -= OnEnemyDeath;
    }

    private void Update()
    {
        slot.SetCardChargedState(ability.CanDeploy());
    }

    private void OnEnemyDeath() => SetText();

    private void OnAbilityUpdated() => SetText();

    private void SetText()
    {
        var needed = ability.Stats.enemyKillNeeded;
        slot.textAuxilliary.text = $"{Mathf.Min(ability.KilledEnemies, needed)}/{needed}";
    }
}
