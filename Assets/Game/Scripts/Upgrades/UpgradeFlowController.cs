using System.Collections.Generic;
using UnityEngine;

public class UpgradeFlowController : MonoBehaviour
{
    [SerializeField] private UpgradeTrigger _upgradeTrigger;
    [SerializeField] private UpgradeSelectionSystem _selectionSystem;
    [SerializeField] private UpgradeUI _upgradeUI;

    [SerializeField] private int _cardsPerChoice;

    private UpgradeRarityCalculator _rarityCalculator = new UpgradeRarityCalculator();
    private int _upgradesTakenCount = 0;

    private void Start()
    {
        _upgradeUI.HideUI();
    }

    private void OnEnable()
    {
        _upgradeTrigger.OnUpgradeTriggered += HandleUpgradeTriggered;
    }

    private void OnDisable()
    {
        _upgradeTrigger.OnUpgradeTriggered -= HandleUpgradeTriggered;
    }

    private void HandleUpgradeTriggered()
    {
        var positiveRarities = new List<EffectRarity>();
        var negativeRarities = new List<EffectRarity>();

        for (int i = 0; i < _cardsPerChoice; i++)
        {
            var rarity = _rarityCalculator.GetPositiveRarity(_upgradesTakenCount);
            positiveRarities.Add(rarity);
            negativeRarities.Add(rarity);
        }

        var cards = _selectionSystem.GenerateCardChoices(positiveRarities, negativeRarities);
        if (cards.Count == 0) return;

        Time.timeScale = 0f;
        _upgradeUI.ShowCards(cards, HandleCardSelected);
        _upgradeUI.ShowUI();
    }

    private void HandleCardSelected(UpgradeCard card)
    {
        _selectionSystem.SelectCard(card);
        _upgradesTakenCount++;

        _upgradeUI.HideUI();

        Time.timeScale = 1f;
    }
}