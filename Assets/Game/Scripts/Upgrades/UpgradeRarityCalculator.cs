using UnityEngine;

public class UpgradeRarityCalculator
{
    public EffectRarity GetPositiveRarity(int upgradesTakenCount)
    {
        float epicChance = Mathf.Clamp01(upgradesTakenCount * 0.05f);
        float rareChance = Mathf.Clamp01(0.3f + upgradesTakenCount * 0.03f);

        float roll = Random.value;
        if (roll < epicChance) return EffectRarity.Epic;
        if (roll < epicChance + rareChance) return EffectRarity.Rare;
        return EffectRarity.Common;
    }
}