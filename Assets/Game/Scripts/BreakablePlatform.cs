using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    private bool _isBroken;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isBroken) return;

        if (!collision.gameObject.TryGetComponent<Player>(out Player player)) return;
        if (!player.IsFalling) return;

        

        _isBroken = true;
        if (PlatformSpawner.Instance != null)
            PlatformSpawner.Instance.DespawnPlatform(gameObject);
    }

    private void OnDisable()
    {
        _isBroken = false;
    }
}
