using UnityEngine;

public class FallingDamageReceiver : MonoBehaviour
{
    private IDamageable _damageable;

    private void Awake()
    {
        _damageable = GetComponent<IDamageable>();

        if (_damageable == null)
        {
            Debug.LogError(
                $"{nameof(FallingDamageReceiver)} requires {nameof(IDamageable)}.",
                this
            );
        }
    }

    public void ReceiveFallingDamage(float damage)
    {
        _damageable?.TakeDamage(damage);
    }
}