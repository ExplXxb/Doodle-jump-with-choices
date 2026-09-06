using UnityEngine;

public enum EffectRarity { Common, Rare, Epic }
public enum EffectIntent { Positive, Negative }

public abstract class EffectSO : ScriptableObject
{
    [SerializeField] private EffectRarity _rarity;
    [SerializeField] private EffectIntent _intent;
    [Tooltip("Назва як частина складеної фрази картки...")]
    [SerializeField] private string _displayName;
    [SerializeField] private string _displayText;

    public EffectRarity Rarity => _rarity;
    public EffectIntent Intent => _intent;
    public string DisplayName => _displayName;
    public string DisplayText => _displayText;

    public abstract void Apply(EffectContext context);
    public abstract void Remove(EffectContext context);
}
