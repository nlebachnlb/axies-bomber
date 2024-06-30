using UnityEngine;

public class AxieHeroDataHolder : MonoBehaviour
{
    public AxieHeroData Data { get; private set; }

    private void Awake()
    {
        EventBus.onSwitchAxieHero += OnSwitchAxieHero;
    }

    private void OnDestroy()
    {
        EventBus.onSwitchAxieHero -= OnSwitchAxieHero;
    }

    private void OnSwitchAxieHero(AxieHeroData axieHeroData)
    {
        Data = axieHeroData;
    }
}
