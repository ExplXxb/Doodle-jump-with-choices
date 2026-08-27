using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour
{
    [SerializeField] private GameObject _deathVFXPrefab;
    [SerializeField] private EnemyVisualEvents _visualEvents;
    [SerializeField] private EnemyAnimator _enemyAnimator;

    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();

        if (_enemyAnimator == null)
        {
            Debug.LogWarning($"{nameof(EnemyDeathHandler)}: відсутній {nameof(EnemyAnimator)} на {name}", this);
            _enemyAnimator = GetComponent<EnemyAnimator>();
        }

        if (_visualEvents == null)
        {
            Debug.LogWarning($"{nameof(EnemyDeathHandler)}: відсутній {nameof(EnemyVisualEvents)} на {name}", this);
            _visualEvents = GetComponentInChildren<EnemyVisualEvents>();
        }

        if (_health == null)
        {
            Debug.LogError($"{nameof(EnemyDeathHandler)}: відсутній {nameof(Health)} на {name}", this);
        }
    }

    private void OnEnable()
    {
        _health.OnDeath += HandleDeath;
        if (_visualEvents != null)
        {
            _visualEvents.OnDeathAnimationFinished += HandleDeathAfterAnimation;
        }
    }

    private void OnDisable()
    {
        _health.OnDeath -= HandleDeath;
        if (_visualEvents != null)
        {
            _visualEvents.OnDeathAnimationFinished -= HandleDeathAfterAnimation;
        }
    }

    private void HandleDeath()
    {
        
    }

    private void HandleDeathAfterAnimation()
    {
        if (_deathVFXPrefab != null)
        {
            Instantiate(_deathVFXPrefab, transform.position, transform.rotation);
        }

        
        _health.Reset();

        if (_enemyAnimator != null)
        {
            _enemyAnimator.ResetAnimator();
        }

        gameObject.SetActive(false);
    }
}