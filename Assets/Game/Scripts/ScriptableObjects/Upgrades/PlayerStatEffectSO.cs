using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Player Stat Effect")]
public class PlayerStatEffectSO : EffectSO
{
    [SerializeField] private float _stompDamageDelta;
    [SerializeField] private float _jumpPowerDelta;
    [SerializeField] private float _movementSpeedDelta;
    [SerializeField] private float _gravityAccelerationDelta;

    public float StompDamageDelta => _stompDamageDelta;
    public float JumpPowerDelta => _jumpPowerDelta;
    public float MovementSpeedDelta => _movementSpeedDelta;
    public float GravityAccelerationDelta => _gravityAccelerationDelta;

    public override void Apply()
    {
        PlayerStats.Instance.AddEffect(this);
    }

    public override void Remove()
    {
        PlayerStats.Instance.RemoveEffect(this);
    }
}