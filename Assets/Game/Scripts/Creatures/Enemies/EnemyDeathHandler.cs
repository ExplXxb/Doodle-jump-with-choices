using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour
{
    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
        if (_health == null)
        {
            Debug.LogError($"{nameof(EnemyDeathHandler)}: відсутній {nameof(Health)} на {name}", this);
        }
    }

    private void OnEnable()
    {
        _health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        _health.OnDeath -= HandleDeath;
    }
    

    private void HandleDeath()
    {
        gameObject.SetActive(false);
    }
}
