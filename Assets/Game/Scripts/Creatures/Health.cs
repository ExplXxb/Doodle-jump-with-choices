using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    public event Action OnDeath;

    [SerializeField] private float _maxHealth = 3;

    private float _currentHealth;
    private bool _isDead = false;

    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _currentHealth;
    public bool IsDead => _isDead;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0)
            return;

        if (_isDead == true)
            return;

        _currentHealth -= damage;
        Debug.Log("Target (" + gameObject + ") take damage, health: " + (_currentHealth + damage) + " => " + _currentHealth);

        if (_currentHealth <= 0)
        {
            Die();
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