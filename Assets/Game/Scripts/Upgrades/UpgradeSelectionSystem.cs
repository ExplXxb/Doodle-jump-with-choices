using System.Collections.Generic;
using UnityEngine;

public class UpgradeSelectionSystem
{
    private readonly UpgradeOptionProvider _optionProvider;

    public UpgradeSelectionSystem(UpgradeOptionProvider optionProvider)
    {
        _optionProvider = optionProvider;
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

            cards.Add(new UpgradeCard(pair[0], pair[1]));
            usedEffects.Add(pair[0]);
            usedEffects.Add(pair[1]);
        }

        return cards;
    }

    public void SelectCard(UpgradeCard card, EffectContext context)
    {
        card.Apply(context);
    }
}
