using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    [SerializeField] private bool _canBeStomped = true;

    private EnemyContactDamage _contactDamage;
    private Collider2D _selfCollider;

    private void Awake()
    {
        _contactDamage = GetComponentInParent<EnemyContactDamage>();
        _selfCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var damageable = collision.GetComponentInParent<IDamageable>();
        if (damageable == null)
            return;

        var player = collision.GetComponentInParent<Player>();
        if (_canBeStomped && player != null && IsStomp(player, collision))
            return;

        _contactDamage.HandleCollision(damageable);
    }

    private bool IsStomp(Player player, Collider2D playerCollider)
    {
        bool isAbove = playerCollider.bounds.min.y >= _selfCollider.bounds.center.y;
        return player.IsFalling && isAbove;
    }
}