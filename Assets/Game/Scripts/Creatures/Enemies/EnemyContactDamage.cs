using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [SerializeField] private float _damage = 1f;

    public void HandleCollision(IDamageable damageable)
    {
        damageable.TakeDamage(new DamageInfo(_damage, transform));
    }
}