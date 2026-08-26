using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private static readonly int DIE_HASH = Animator.StringToHash("Die");
    private static readonly int SPEED_HASH = Animator.StringToHash("Speed");

    [SerializeField] private Animator _animator;

    private Health _health;
    private EnemyMovement _movement;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _movement = GetComponent<EnemyMovement>();
    }

    private void OnEnable()
    {
        if (_health != null)
            _health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        if (_health != null)
            _health.OnDeath -= HandleDeath;
    }

    private void Update()
    {
        if (_movement != null)
        {
            _animator.SetFloat(SPEED_HASH, Mathf.Abs(_movement.CurrentSpeed));
        }
    }

    private void HandleDeath()
    {
        _animator.SetTrigger(DIE_HASH);
    }
}
