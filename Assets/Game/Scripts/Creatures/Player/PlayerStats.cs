using System.Collections.Generic;

public class PlayerStats
{
    private const float BASE_JUMP_POWER = 18f;
    private const float BASE_MOVEMENT_SPEED = 10f;
    private const float BASE_STOMP_DAMAGE = 1f;
    private const float BASE_GRAVITY = 30f;

    private const float JUMP_POWER_UPGRADE_MULTIPLIER = 1f;
    private const float MOVEMENT_SPEED_UPGRADE_MULTIPLIER = 0.5f;
    private const float STOMP_DAMAGE_UPGRADE_MULTIPLIER = 1f;

    private readonly float _baseJumpPower;
    private readonly float _baseMovementSpeed;
    private readonly float _baseGravityAcceleration;
    private readonly float _baseStompDamage;

    private List<PlayerStatEffectSO> _activeEffects = new List<PlayerStatEffectSO>();

    public float JumpPower { get; private set; }
    public float MovementSpeed { get; private set; }
    public float GravityAcceleration { get; private set; }
    public float StompDamage { get; private set; }

    public PlayerStats(MetaProgressService metaProgress)
    {
        _baseJumpPower = BASE_JUMP_POWER + metaProgress.GetCurrentLevel(ShopLotType.JumpPower) * JUMP_POWER_UPGRADE_MULTIPLIER;
        _baseMovementSpeed = BASE_MOVEMENT_SPEED + metaProgress.GetCurrentLevel(ShopLotType.MovementSpeed) * MOVEMENT_SPEED_UPGRADE_MULTIPLIER;
        _baseGravityAcceleration = BASE_GRAVITY;
        _baseStompDamage = BASE_STOMP_DAMAGE + metaProgress.GetCurrentLevel(ShopLotType.StompDamage) * STOMP_DAMAGE_UPGRADE_MULTIPLIER;
        
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