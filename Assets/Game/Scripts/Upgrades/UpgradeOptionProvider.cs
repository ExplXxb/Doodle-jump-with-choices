using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeOptionProvider
{
    private readonly List<EffectSO> _allEffects;

    public UpgradeOptionProvider(List<EffectSO> allEffects)
    {
        _allEffects = allEffects;
    }

    public List<EffectSO> GetChoices(EffectRarity positiveRarity, EffectRarity negativeRarity, HashSet<EffectSO> excluded)
    {
        var positivePool = _allEffects.Where(e => e.Intent == EffectIntent.Positive && e.Rarity == positiveRarity && !excluded.Contains(e)).ToList();
        var negativePool = _allEffects.Where(e => e.Intent == EffectIntent.Negative && e.Rarity == negativeRarity && !excluded.Contains(e)).ToList();

        var result = new List<EffectSO>();
        if (positivePool.Count > 0) result.Add(positivePool[Random.Range(0, positivePool.Count)]);
        if (negativePool.Count > 0) result.Add(negativePool[Random.Range(0, negativePool.Count)]);

        return result;
    }
}
