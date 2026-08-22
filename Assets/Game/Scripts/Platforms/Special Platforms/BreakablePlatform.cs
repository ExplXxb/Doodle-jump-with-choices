using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent<Player>(out Player player)) return;
        if (!player.IsFalling) return;

        if (PlatformSpawner.Instance != null)
            PlatformSpawner.Instance.DespawnPlatform(gameObject);
    }
}
