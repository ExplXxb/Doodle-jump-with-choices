public class UpgradeCard
{
    public EffectSO PositiveEffect { get; }
    public EffectSO NegativeEffect { get; }

    public UpgradeCard(EffectSO positiveEffect, EffectSO negativeEffect)
    {
        PositiveEffect = positiveEffect;
        NegativeEffect = negativeEffect;
    }

    public void Apply(EffectContext context)
    {
        PositiveEffect?.Apply(context);
        NegativeEffect?.Apply(context);
    }
}