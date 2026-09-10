using System.Collections.Generic;
using UnityEngine;

public class UpgradeSelectionSystem
{
    private readonly UpgradeOptionProvider _optionProvider;
    private readonly IAnalyticsService _analyticsService;

    public UpgradeSelectionSystem(UpgradeOptionProvider optionProvider, IAnalyticsService analyticsService)
    {
        _optionProvider = optionProvider;
        _analyticsService = analyticsService;
    }

    public List<UpgradeCard> GenerateCardChoices(List<EffectRarity> positiveRarities, List<EffectRarity> negativeRarities)
    {
        var cards = new List<UpgradeCard>();
        var usedEffects = new HashSet<EffectSO>();

        int count = Mathf.Min(positiveRarities.Count, negativeRarities.Count);

        for (int i = 0; i < count; i++)
        {
            var pair = _optionProvider.GetChoices(positiveRarities[i], negativeRarities[i], usedEffects);
            if (pair.Count < 2) continue;

            var newCard = new UpgradeCard(pair[0], pair[1]);
            cards.Add(newCard);
            usedEffects.Add(pair[0]);
            usedEffects.Add(pair[1]);

            _analyticsService.LogCardSpawned(newCard);
        }

        return cards;
    }

    public void SelectCard(UpgradeCard card, EffectContext context)
    {
        card.Apply(context);

        _analyticsService.LogCardSelected(card);
    }
}
