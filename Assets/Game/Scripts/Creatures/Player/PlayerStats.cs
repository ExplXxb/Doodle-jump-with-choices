using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    private readonly float _baseJumpPower = 18f;
    private readonly float _baseMovementSpeed = 10f;
    private readonly float _baseGravityAcceleration = 30f;
    private readonly float _baseStompDamage = 1f;

    private List<PlayerStatEffectSO> _activeEffects = new List<PlayerStatEffectSO>();

    public float JumpPower { get; private set; }
    public float MovementSpeed { get; private set; }
    public float GravityAcceleration { get; private set; }
    public float StompDamage { get; private set; }

    public PlayerStats()
    {
        RecalculateStats();
    }

    public void AddEffect(PlayerStatEffectSO effect)
    {
        _activeEffects.Add(effect);
        RecalculateStats();
    }

    public void RemoveEffect(PlayerStatEffectSO effect)
    {
        _activeEffects.Remove(effect);
        RecalculateStats();
    }

    private void RecalculateStats()
    {
        JumpPower = _baseJumpPower + SumDelta(e => e.JumpPowerDelta);
        MovementSpeed = _baseMovementSpeed + SumDelta(e => e.MovementSpeedDelta);
        GravityAcceleration = _baseGravityAcceleration + SumDelta(e => e.GravityAccelerationDelta);
        StompDamage = _baseStompDamage + SumDelta(e => e.StompDamageDelta);
    }

    private float SumDelta(System.Func<PlayerStatEffectSO, float> selector)
    {
        float sum = 0f;
        foreach (var effect in _activeEffects) sum += selector(effect);
        return sum;
    }
}