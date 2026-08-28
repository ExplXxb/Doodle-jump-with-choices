using UnityEngine;

public class PlayerHitReaction : MonoBehaviour
{
    [SerializeField] private float _knockbackForce = 8f;
    [SerializeField] private float _knockbackDrag = 12f;
    [SerializeField] private float _hitStunDuration = 0.15f;
    [SerializeField] private float _controlRampDuration = 0.3f;
    [SerializeField] private float _invulnerabilityDuration = 0.5f;

    private Health _health;
    private Vector2 _knockbackVelocity;
    private float _hitStunTimer;
    private float _rampTimer;
    private float _invulnerabilityTimer;

    public bool IsInvulnerable => _invulnerabilityTimer > 0f;
    public float InputMultiplier { get; private set; } = 1f;
    public Vector2 KnockbackDisplacement { get; private set; }

    private void Awake()
    {
        _health = GetComponent<Health>();
        if (_health == null)
            Debug.LogError($"{nameof(PlayerHitReaction)}: відсутній {nameof(Health)} на {name}", this);
    }

    public void ApplyHit(Vector2 sourcePosition, Vector2 selfPosition)
    {
        if (IsInvulnerable) return;

        Vector2 direction = (selfPosition - sourcePosition).normalized;
        _knockbackVelocity = direction * _knockbackForce;
        _hitStunTimer = _hitStunDuration;
        _rampTimer = 0f;
        InputMultiplier = 0f;

        _invulnerabilityTimer = _invulnerabilityDuration;
        _health.SetInvulnerable(true);
    }

    public void Tick(float deltaTime)
    {
        if (_hitStunTimer > 0f)
        {
            _hitStunTimer -= deltaTime;
        }
        else if (InputMultiplier < 1f)
        {
            _rampTimer += deltaTime;
            InputMultiplier = Mathf.Clamp01(_rampTimer / _controlRampDuration);
        }

        _knockbackVelocity = Vector2.MoveTowards(_knockbackVelocity, Vector2.zero, _knockbackDrag * deltaTime);
        KnockbackDisplacement = _knockbackVelocity * deltaTime;

        if (IsInvulnerable)
        {
            _invulnerabilityTimer -= deltaTime;

            if (_invulnerabilityTimer <= 0f)
            {
                _health.SetInvulnerable(false);
            }
        }
    }
}
