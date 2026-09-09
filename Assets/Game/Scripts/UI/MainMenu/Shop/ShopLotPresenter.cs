using UnityEditor.MPE;
using UnityEngine;

public class ShopLotPresenter
{
    private readonly ShopLotView _view;
    private readonly ShopLotType _type;
    private readonly UpgradeLotSettings _settings;
    private readonly MetaProgressService _progressService;

    public ShopLotPresenter(ShopLotView view, ShopLotType type, UpgradeLotSettings settings, MetaProgressService progressService)
    {
        _view = view;
        _type = type;
        _settings = settings;
        _progressService = progressService;

        _view.OnBuyClicked += HandleBuyRequest;
        UpdateView();
    }

    public void UpdateView()
    {
        int currentLevel = _progressService.GetCurrentLevel(_type);
        int maxLevel = _settings.MaxLevel;
        int totalGold = _progressService.TotalGold;

        bool isMaxLevel = currentLevel >= maxLevel;

        int currentUpgradeCost = isMaxLevel ? 0 : _settings.UpgradeCosts[currentLevel];

        string costText = isMaxLevel ? "MAX" : currentUpgradeCost.ToString();

        bool iconIsActive = !isMaxLevel && totalGold >= currentUpgradeCost;

        _view.Render(_settings.ShopLotIcon, iconIsActive, costText, currentLevel, maxLevel);
    }


    private void HandleBuyRequest()
    {
        if (_progressService.TryBuyShopLot(_type))
        {
            UpdateView();
        }
        else
        {
            Debug.Log($"Неможливо купити {_type}. Недостатньо золота або макс. рівень.");
        }
    }
}
