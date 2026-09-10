using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UpgradeLotSettings
{
    public Sprite ShopLotIcon;
    public ShopLotType UpgradeType;
    public int MaxLevel = 5;

    [Tooltip("Ціна переходу на новий рівень апгрейду. Індекс 0 = ціна с 0 на 1 рівень, індекс 1 = з 1 на 2 і т.д.")]
    public List<int> UpgradeCosts = new();
}

[CreateAssetMenu(fileName = "ShopConfig", menuName = "Configs/Shop Config")]
public class ShopConfig : ScriptableObject
{
    [SerializeField] private List<UpgradeLotSettings> _lots = new();

    public Dictionary<ShopLotType, UpgradeLotSettings> Upgrades { get; private set; } = new();

    public void Initialize()
    {
        Upgrades.Clear();
        foreach (var lot in _lots)
        {
            if (lot != null && !Upgrades.ContainsKey(lot.UpgradeType))
            {
                Upgrades.Add(lot.UpgradeType, lot);
            }
        }
    }
}