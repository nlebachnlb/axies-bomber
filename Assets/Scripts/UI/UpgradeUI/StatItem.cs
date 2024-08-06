using System.Collections;
using System.Collections.Generic;
using static AxieUpgradeConfig;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatItem : MonoBehaviour
{
    [SerializeField] private Stat statType;
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI statValue;
    [SerializeField] private TextMeshProUGUI upgradePrice;
    [SerializeField] private Button upgradeButton;

    public Stat Stat => statType;

    private int id;
    private int level;
    private float baseValue;
    private AxiePackedConfig axiePackedConfig;

    private int currentPrice;
    private bool isMaxedLevel;

    private void Awake()
    {
        statName.text = (statType switch
        {
            Stat.Speed => "Move Speed",
            Stat.Health => "Health",
            Stat.BombMagazine => "Bomb Count",
            Stat.BombExplosionRadius => "Bomb Range",
            _ => ""
        }).ToUpper();
    }

    private void OnEnable()
    {
        upgradeButton.onClick.AddListener(OnClick_UpgradeButton);
        EventBus.onGearChanged += OnGearChanged;
    }

    private void OnDisable()
    {
        upgradeButton.onClick.RemoveListener(OnClick_UpgradeButton);
        EventBus.onGearChanged -= OnGearChanged;
    }

    public void Init(int axieId, int currentStatLevel)
    {
        id = axieId;
        level = currentStatLevel;
        axiePackedConfig = AppRoot.Instance.Config.availableAxies.GetAxiePackedConfigById(axieId);
        baseValue = axiePackedConfig.axieStats.GetBaseValue(Stat);
    }

    public void LoadUI()
    {
        GetUpgradeData((AxieIdentity)id,
                       statType,
                       level,
                       out float currentUpgradeDelta,
                       out float nextUpgradeDelta,
                       out isMaxedLevel,
                       out currentPrice);

        float currentValue = baseValue + currentUpgradeDelta;
        if (isMaxedLevel)
        {
            statValue.text = $"<color=#2CC4E5>{currentValue:F0}</color>";
        }
        else
        {
            float nextValue = baseValue + nextUpgradeDelta;
            statValue.text = $"{currentValue:F0} -> <color=#22C49A>{nextValue:F0}</color>";
        }
        upgradeButton.gameObject.SetActive(!isMaxedLevel);
        LoadPriceText();
    }

    private void LoadPriceText()
    {
        string priceText = currentPrice.ToString();
        if (AppRoot.Instance.UserDataModel.User.Gear < currentPrice)
            priceText = $"<color=#FF5F00>{priceText}</color>";
        upgradePrice.text = isMaxedLevel ? "<color=#2CC4E5>MAXED</color>" :
                                           $"{priceText} <sprite index=0>";
    }

    private void OnClick_UpgradeButton()
    {
        if (!isMaxedLevel && AppRoot.Instance.UserDataModel.User.Gear >= currentPrice)
        {
            AppRoot.Instance.UserDataModel.User.Gear -= currentPrice;
            level = AppRoot.Instance.UserDataModel.IncreaseStatLevel(id, statType);
            LoadUI();
        }
    }

    private void OnGearChanged(int obj)
    {
        LoadPriceText();
    }

    private void GetUpgradeData(AxieIdentity id, Stat stat, int statLevel, out float currentUpgradeDelta, out float nextUpgradeDelta, out bool isMaxedLevel, out int upgradePrice)
    {
        AxieUpgradeConfig axieUpgradeConfig = AppRoot.Instance.Config.axieUpgrades;
        DataPacket dataPacket = axieUpgradeConfig.GetDataPacket(id, stat, statLevel);

        currentUpgradeDelta = dataPacket.UpgradeValue;
        upgradePrice = dataPacket.UpgradeCost;

        int maxLevel = axieUpgradeConfig.GetStatMaxLevel(id, stat);
        isMaxedLevel = level == maxLevel;
        if (isMaxedLevel)
        {
            nextUpgradeDelta = currentUpgradeDelta;
        }
        else
        {
            DataPacket nextLevelPacket = axieUpgradeConfig.GetDataPacket(id, statType, this.level + 1);
            nextUpgradeDelta = nextLevelPacket.UpgradeValue;
        }
    }
}
