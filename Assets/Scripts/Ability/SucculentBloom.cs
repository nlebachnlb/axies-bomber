using UnityEngine;

public class SucculentBloom : AxieAbility<SucculentBloomStats>
{
    [SerializeField] private ParticleSystem effect;

    private void OnEnable()
    {
        EventBus.onRoomClear += OnRoomClear;
    }

    private void OnDisable()
    {
        EventBus.onRoomClear -= OnRoomClear;
    }

    private void OnRoomClear()
    {
        if (Random.Range(0f, 1f) < Stats.Rate)
        {
            Debug.Log($"[Ability] SucculentBloom HP Buff: {Stats.HpBuff}");
            controller.AxieHeroData.axieStats.AddBuff(new GenericStatBuff() { Stat = Stat.Health, BuffValue = Stats.HpBuff });
            controller.AxieHeroData.RaiseUpdateInfo();
        }
    }
}
