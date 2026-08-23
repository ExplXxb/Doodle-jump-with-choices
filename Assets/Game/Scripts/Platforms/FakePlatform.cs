using UnityEngine;

public class FakePlatform : MonoBehaviour, IPlatformBehavior
{
    public bool ShouldJump { get; private set; } = false;
    public void OnPlayerLanded()
    {
        if (PlatformSpawner.Instance != null)
            PlatformSpawner.Instance.DespawnPlatform(gameObject);
    }
}
