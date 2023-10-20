using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [Header("Player")]
    public AxieConfigReader playerConfig;
    public StatsModifier statsModifier;

    [Header("Config & Stats")]
    public AxieConfig axieConfig;
    public AxieStats axieBaseStats;
    public BombStats bombBaseStats;

    [Header("Axie Heroes Slots")]
    public List<AxieHeroConfig> slots;
    public List<KeyCode> inputSlotMap;

    [Header("Camera")]
    [SerializeField] private CameraShake cameraShaker;

    private int currentSlot = 0;

    private void Awake()
    {
        EventBus.onBombFuse += PlayShake;
        ReloadAxieHeroConfig();
    }

    private void Start()
    {
        
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

        AxieHeroConfig hero = slots[slotIndex];
        axieBaseStats = hero.axieStats;
        bombBaseStats = hero.bombStats;
        axieConfig = hero.axieConfig;

        ReloadAxieHeroConfig();

        currentSlot = slotIndex;
    }

    private void ReloadAxieHeroConfig()
    {
        playerConfig.Load(axieConfig);
        statsModifier.axieStats = axieBaseStats;
        statsModifier.bombStats = bombBaseStats;
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
