using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

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

    [Header("Environment")]
    [SerializeField] private Light globalLight;
    [SerializeField] private SkillPoolEntrance item;

    [Header("HUD")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private AxieHeroHUD axieHeroHUD;
    [SerializeField] private SkillPickUI skillPickUI;
    [SerializeField] private GameObject clearBannerHUD;
    [SerializeField] private AbilityHUD abilityHUD;

    [Header("Test mode")]
    [SerializeField] private bool testMap;
    [SerializeField] private string testMapId;

    private int currentSlot = -1;
    private MapController mapController;
    private SkillPoolController skillController;

    private void Awake()
    {
        EventBus.onBombFuse += PlayShake;
        EventBus.onAxieHeroDeath += OnAxieHeroDeath;
        EventBus.onEnterSkillPool += OnEnterSkillPool;
        EventBus.onOpenSkillPool += OnOpenSkillPool;
        EventBus.onPickSkill += OnPickSkill;
        EventBus.onRoomClear += OnRoomClear;

        mapController = GetComponent<MapController>();
        skillController = GetComponent<SkillPoolController>();
    }

    private void OnDestroy()
    {
        EventBus.onBombFuse -= PlayShake;
        EventBus.onAxieHeroDeath -= OnAxieHeroDeath;
        EventBus.onEnterSkillPool -= OnEnterSkillPool;
        EventBus.onOpenSkillPool -= OnOpenSkillPool;
        EventBus.onPickSkill -= OnPickSkill;
        EventBus.onRoomClear -= OnRoomClear;
    }

    private void Start()
    {
        if (!testMap)
            InitAxieHeroDataSlots();

        axieHeroHUD.InitHUD(slots, inputSlotMap);
        foreach (AxieHeroData slot in slots)
            slot.ReloadInGameData();

        SwitchAxieHero(0);
        mapController.Reload("1-1");
    }

    private void InitAxieHeroDataSlots()
    {
        slots = new List<AxieHeroData>();
        UserData model = AppRoot.Instance.UserDataModel.User;
        List<AxiePackedConfig> configs = model.currentPickedAxies.Select(id => AppRoot.Instance.Config.availableAxies.GetAxiePackedConfigById(id)).ToList();

        Debug.Log("Initiating...");
        for (int i = 0; i < configs.Count; ++i)
        {
            AxieHeroData axie = new AxieHeroData();
            axie.identity = (AxieIdentity)configs[i].id;
            axie.axieConfig = Instantiate(configs[i].axieConfig);
            axie.axieStats = Instantiate(configs[i].axieStats);
            axie.bombStats = Instantiate(configs[i].bombStats);
            axie.abilityPrefab = configs[i].abilityPrefab;
            axie.ability = null;

            Debug.Log("Init: " + axie.identity);
            axie.axieStats.ResetBuffs();
            slots.Add(axie);
        }
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
        axieHeroHUD.SelectSlot(slotIndex);
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

        if (Input.GetKeyDown(KeyCode.C))
            Instantiate(item);
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

    private void OnOpenSkillPool(bool isAbilityPool)
    {
        List<SkillConfig> skills = skillController.GetRandomizedSkillPool(isAbilityPool);
        EventBus.RaiseOnEnterSkillPool(skills);
    }

    private void OnEnterSkillPool(List<SkillConfig> skills)
    {
        StartCoroutine(OnEnterSkillPoolProcess(skills));
    }

    private IEnumerator OnEnterSkillPoolProcess(List<SkillConfig> skills)
    {
        cameraShaker.Shake(0.5f, 0.3f);
        globalLight.DOIntensity(0f, 0.15f).OnComplete(() =>
        {
            globalLight.DOIntensity(1f, 0.45f);
        });

        yield return new WaitForSeconds(0.5f);

        SkillPickUI ui = Instantiate(skillPickUI, canvas.transform);
        ui.skills = skills;
    }

    private void OnPickSkill(SkillConfig skill)
    {
        if (skill.isAbility)
        {
            foreach (AxieHeroData axie in slots)
            {
                if (axie.identity == skill.axieIdentity)
                {
                    axie.ability = skill;
                    EventBus.RaiseOnSwitchAxieHero(slots[currentSlot]);
                    break;
                }
            }
        }
        else
        {
            StatsBuff buff = (StatsBuff)skill;
            Debug.Log("Skill is for " + buff.axieIdentity + ", buff type is " + buff.buffType);
            foreach (AxieHeroData axie in slots)
            {
                if (axie.identity == buff.axieIdentity)
                {
                    axie.axieStats.AddBuff(buff);
                    Debug.Log("After buff for: " + axie.identity + ": " + axie.axieStats.Calculate().ToString());
                    axie.bombsRemaining = axie.axieStats.Calculate().bombMagazine;
                    axie.RaiseUpdateInfo();
                    break;
                }
            }
        }
    }

    private void OnRoomClear()
    {
        Instantiate(clearBannerHUD, canvas.transform);
    }
}
