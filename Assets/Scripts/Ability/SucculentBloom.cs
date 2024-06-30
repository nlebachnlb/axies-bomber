using UnityEngine;

public class SucculentBloom : AxieAbility<SucculentBloomStats>
{
    [SerializeField] private ParticleSystem effect;

    private AxieHeroDataHolder axieHeroDataHolder;

    private void OnEnable()
    {
        EventBus.onRoomClear += OnRoomClear;
    }

    private void OnDisable()
    {
        EventBus.onRoomClear -= OnRoomClear;
    }

    public override void AssignOwner(GameObject owner)
    {
        base.AssignOwner(owner);
        axieHeroDataHolder = owner.GetComponent<AxieHeroDataHolder>();
    }

    private void OnRoomClear()
    {
        if (true || Random.Range(0f, 1f) < Stats.Rate)
        {
            Debug.Log($"[Ability] SucculentBloom HP Buff: {Stats.HpBuff}");
            axieHeroDataHolder.Data.axieStats.AddBuff(new GenericStatBuff() { Stat = Stat.Health, BuffValue = Stats.HpBuff });
            axieHeroDataHolder.Data.RaiseUpdateInfo();
        }
    }
}
