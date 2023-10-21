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

    private int currentSlot = -1;

    private void Awake()
    {
        EventBus.onBombFuse += PlayShake;
        EventBus.onAxieHeroDeath += OnAxieHeroDeath;
    }

    private void Start()
    {
        axieHeroHUD.InitHUD(slots, inputSlotMap);
        foreach (AxieHeroData slot in slots)
            slot.ReloadInGameData();

        SwitchAxieHero(0);
    }

    public void PlayShake()
    {
        cameraShaker.Shake(0.2f, 0.3f);
    }

    public void SwitchAxieHero(int slotIndex)
    {
        if (slotIndex >= slots.Count || currentSlot == slotIndex)
        {
            return;
        }

        AxieHeroData hero = slots[slotIndex];
        if (hero.IsDead)
        {
            return;
        }

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

    private int GetFirstAliveSlotFromCurrent()
    {
        int slot = currentSlot;
        int cnt = 0;
        while (cnt < slots.Count)
        {
            if (!slots[slot].IsDead)
            {
                return slot;
            }
            slot = (slot + 1) % slots.Count;
            cnt++;
        }

        return -1;
    }

    private void OnAxieHeroDeath(AxieHeroData axieHeroData)
    {
        int aliveSlot = GetFirstAliveSlotFromCurrent();
        if (aliveSlot > 0)
        {
            SwitchAxieHero(aliveSlot);
        }
        else
        {
            Debug.Log("Game Over");
        }
    }
}
