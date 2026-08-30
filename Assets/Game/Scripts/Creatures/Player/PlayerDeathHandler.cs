using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    private const string DEATH = "Death";
    private const float TIME_SCALE_EPSILON = 0.001f;

    public event Action OnDeathUIStart;

    [SerializeField] private List<GameObject> _deathVFXPrefabs;
    [SerializeField] private PlayerSFX _playerSFX;
    [SerializeField] private SpriteRenderer _playerSpriteRenderer;

    [SerializeField] private float _timeSlowingStep = 0.05f;
    [SerializeField] private float _timeSlowingInterval = 0.05f;

    [SerializeField, Min(0f)]
    private float _deathUIStartDelay = 0.2f;

    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>() ?? GetComponentInChildren<Health>();

        if (_health == null )
        {
            Debug.LogError($"{nameof(PlayerDeathHandler)}: відсутній {nameof(Health)} на {name}", this);
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
        _playerSFX.PlaySound(DEATH);

        foreach (var deathVFX in _deathVFXPrefabs)
        {
            Instantiate(deathVFX, transform.position, transform.rotation);
        }

        _playerSpriteRenderer.gameObject.SetActive(false);

        StartCoroutine(DeathSequenceRoutine());
    }

    private IEnumerator DeathSequenceRoutine()
    {
        StartCoroutine(DeathTimeFreezeRoutine());

        yield return new WaitForSecondsRealtime(_deathUIStartDelay);

        OnDeathUIStart?.Invoke();
    }

    private IEnumerator DeathTimeFreezeRoutine()
    {
        float realInterval = _timeSlowingInterval;
        float lastLogTime = Time.unscaledTime;

        while (Time.timeScale > TIME_SCALE_EPSILON)
        {
            Time.timeScale = Mathf.Max(0, Time.timeScale - _timeSlowingStep);

            if (Time.timeScale <= TIME_SCALE_EPSILON)
            {
                Time.timeScale = 0f;
                break;
            }

            realInterval = _timeSlowingInterval / Time.timeScale;

            float elapsed = Time.unscaledTime - lastLogTime;
            Debug.Log($"timeScale: {Time.timeScale}, прошло реального времени: {elapsed}");
            lastLogTime = Time.unscaledTime;

            yield return new WaitForSecondsRealtime(realInterval);
        }

        Time.timeScale = 0f;
        Debug.Log($"Вышли с while, timeScale: {Time.timeScale}");
    }
}
