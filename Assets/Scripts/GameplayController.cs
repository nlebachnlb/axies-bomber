using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [Header("Player")]
    public AxieConfigReader playerConfig;

    [Header("Config & Stats")]
    public AxieConfig axieConfig;
    public AxieStats axieBaseStats;
    public BombStats bombBaseStats;

    [Header("Axie Heroes Slots")]
    public List<AxieHeroData> slots;
    public List<KeyCode> inputSlotMap;
    public AxieHeroData CurrentAxieHeroData { get => slots[currentSlot]; }

    [Header("Camera")]
    [SerializeField] private CameraShake cameraShaker;

    [Header("HUD")]
    [SerializeField] private AxieHeroHUD axieHeroHUD;

    private int currentSlot = 0;

    private void Awake()
    {
        EventBus.onBombFuse += PlayShake;
        ReloadAxieHeroConfig();
    }

    private void Start()
    {
        SwitchAxieHero(0);

        axieHeroHUD.InitHUD(slots, inputSlotMap);
        foreach (AxieHeroData slot in slots)
            slot.ReloadInGameData();
    }

    public void PlayShake()
    {
        cameraShaker.Shake(0.2f, 0.2f);
    }

    public void SwitchAxieHero(int slotIndex)
    {
        if (slotIndex >= slots.Count)
        {
            return;
        }

        AxieHeroData hero = slots[slotIndex];
        axieBaseStats = Instantiate(hero.axieStats);
        bombBaseStats = Instantiate(hero.bombStats);
        axieConfig = Instantiate(hero.axieConfig);

        ReloadAxieHeroConfig();

        currentSlot = slotIndex;
        EventBus.RaiseOnSwitchAxieHero(hero);
    }

    private void ReloadAxieHeroConfig()
    {
        playerConfig.Load(axieConfig);
    }

    private void Update()
    {
        for (int i = 0; i < inputSlotMap.Count; ++i)
        {
            if (Input.GetKeyDown(inputSlotMap[i]))
            {
                SwitchAxieHero(i);
            }
        }
    }
}
