using UnityEngine;

public class OneJumpPlatform : MonoBehaviour, IPlatformBehavior
{
    public bool ShouldJump { get; private set; } = true;
    public void OnPlayerLanded()
    {
        if (PlatformSpawner.Instance != null)
            PlatformSpawner.Instance.DespawnPlatform(gameObject);
    }
}