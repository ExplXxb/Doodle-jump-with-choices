using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    public event Action<DamageInfo> OnTakeDamage;
    public event Action OnDeath;

    [SerializeField] private float _maxHealth = 3;

    private float _currentHealth;
    private bool _isDead = false;
    private bool _isInvulnerable;

    public void SetInvulnerable(bool value) => _isInvulnerable = value;

    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _currentHealth;
    public bool IsDead => _isDead;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        if (_isInvulnerable) 
            return;

        if (damageInfo.Amount <= 0)
            return;

        if (_isDead == true)
            return;

        _currentHealth -= damageInfo.Amount;
        Debug.Log("Target (" + gameObject + ") take damage, health: " + (_currentHealth + damageInfo.Amount) + " => " + _currentHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
        else
        {
            OnTakeDamage?.Invoke(damageInfo);
        }
    }

    public void Kill()
    {
        if (_isDead) return;
        _currentHealth = 0;
        Die();
    }

    public void Reset()
    {
        _currentHealth = _maxHealth;
        _isDead = false;
    }

    private void Die()
    {
        _isDead = true;
        OnDeath?.Invoke();
    }
}