using UnityEngine;

public struct DamageInfo
{
    public float Amount;
    public Vector2 SourcePosition;

    public DamageInfo(float amount, Vector2 sourcePosition)
    {
        Amount = amount;
        SourcePosition = sourcePosition;
    }

    public DamageInfo(float amount, Transform transform) : this(amount, transform.position) { }
}
