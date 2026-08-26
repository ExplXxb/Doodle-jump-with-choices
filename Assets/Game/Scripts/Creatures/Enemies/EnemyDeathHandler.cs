using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour
{
    [SerializeField] private GameObject _deathVFXPrefab;
    [SerializeField] private EnemyVisualEvents _visualEvents;

    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();

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

        gameObject.SetActive(false);
    }
}