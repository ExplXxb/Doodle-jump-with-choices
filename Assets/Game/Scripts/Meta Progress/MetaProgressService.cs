using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public enum ShopLotType
{
    JumpPower,
    MovementSpeed,
    StompDamage
}

public class MetaProgressService
{
    public event Action<int> OnTotalGoldChanged;

    private int _totalGold;
    private Dictionary<ShopLotType, UpgradeLotSettings> _shopUpgradesSettings;

    private Dictionary<ShopLotType, int> _currentLevels = new();

    public int TotalGold
    {
        get => _totalGold;
        private set
        {
            _totalGold = value;
            OnTotalGoldChanged?.Invoke(_totalGold);
        }
    }

    [Inject]
    public void Construct(ShopConfig shopConfig)
    {
        _shopUpgradesSettings = shopConfig.Upgrades;
        LoadProgress();
    }

    public int GetCurrentLevel(ShopLotType type) => _currentLevels.GetValueOrDefault(type, 0);
    public int GetMaxLevel(ShopLotType type) => _shopUpgradesSettings.TryGetValue(type, out var s) ? s.MaxLevel : 0;

    public bool TryBuyShopLot(ShopLotType upgradeType)
    {
        if (GetCurrentLevel(upgradeType) >= GetMaxLevel(upgradeType)) return false;

        int cost =  CurrentUpgradeCost(upgradeType);
        if (TotalGold >= cost)
        {
            TotalGold -= cost;
            _currentLevels[upgradeType]++;
            SaveProgress();
            return true;
        }
        return false;
    }

    private int CurrentUpgradeCost(ShopLotType upgradeType)
    {
        int currentLevel = GetCurrentLevel(upgradeType);
        var settings = _shopUpgradesSettings[upgradeType];

        if (currentLevel >= settings.UpgradeCosts.Count)
            return 0;

        return settings.UpgradeCosts[currentLevel];
    }

    private void SaveProgress()
    {
        foreach (var pair in _currentLevels)
        {
            PlayerPrefs.SetInt("Meta_" + pair.Key + "Level", pair.Value);
        }

        PlayerPrefs.SetInt("Meta_Gold", TotalGold);
        PlayerPrefs.Save();
        Debug.Log("Мета-прогрес збережено!");
    }

    private void LoadProgress()
    {
        foreach (var pair in _shopUpgradesSettings)
        {
            int savedLevel = PlayerPrefs.GetInt("Meta_" + pair.Key + "Level", 0);
            _currentLevels[pair.Key] = savedLevel;
        }

        TotalGold = PlayerPrefs.GetInt("Meta_Gold", 1000);
        Debug.Log("Мета-прогрес завантажено!");
    }
}
