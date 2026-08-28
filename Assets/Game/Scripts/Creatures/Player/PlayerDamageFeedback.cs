using System.Collections;
using UnityEngine;

public class PlayerDamageFeedback : MonoBehaviour
{
    private const string HIT = "Hit";

    [SerializeField] private Health _health;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private PlayerSFX _playerSFX;
    [SerializeField] private float _blinkInterval = 0.08f;
    [SerializeField] private float _blinkDuration = 0.5f;

    private Coroutine _blinkRoutine;

    private void Awake()
    {
        if (_health == null)
            _health = GetComponent<Health>();

        if (_spriteRenderer == null) 
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (_playerSFX == null)
            _playerSFX = GetComponent<PlayerSFX>();
    }

    private void OnEnable()
    {
        _health.OnTakeDamage += HandleTakeDamage;
        _health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        _health.OnTakeDamage -= HandleTakeDamage;
        _health.OnDeath -= HandleDeath;
    }

    private void HandleTakeDamage(DamageInfo damageInfo)
    {
        _playerSFX.PlaySound(HIT);

        if (_blinkRoutine != null)
            StopCoroutine(_blinkRoutine);

        _blinkRoutine = StartCoroutine(BlinkRoutine());
    }

    private void HandleDeath()
    {
        if (_blinkRoutine != null)
            StopCoroutine(_blinkRoutine);

        _spriteRenderer.enabled = true;
    }

    private IEnumerator BlinkRoutine()
    {
        float elapsed = 0f;

        while (elapsed < _blinkDuration)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            yield return new WaitForSeconds(_blinkInterval);
            elapsed += _blinkInterval;
        }

        _spriteRenderer.enabled = true;
    }
}
