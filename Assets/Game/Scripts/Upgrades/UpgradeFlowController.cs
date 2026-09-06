using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class UpgradeFlowController : MonoBehaviour
{
    [SerializeField] private UpgradeTrigger _upgradeTrigger;
    [SerializeField] private UpgradeUI _upgradeUI;
    [SerializeField] private int _cardsPerChoice = 3;

    private UpgradeSelectionSystem _selectionSystem;
    private EffectContext _effectContext;

    private readonly UpgradeRarityCalculator _rarityCalculator = new UpgradeRarityCalculator();
    private int _upgradesTakenCount = 0;

    [Inject]
    public void Construct(UpgradeSelectionSystem selectionSystem)
    {
        _selectionSystem = selectionSystem;
    }

    public void SetContext(EffectContext effectContext)
    {
        _effectContext = effectContext;
    }

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
        _selectionSystem.SelectCard(card, _effectContext);
        _upgradesTakenCount++;

        _upgradeUI.HideUI();
        Time.timeScale = 1f;
    }
}
