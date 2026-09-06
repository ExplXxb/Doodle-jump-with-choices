using System;
using UnityEngine;

public class OneJumpPlatform : MonoBehaviour, IPlatformBehavior, IDespawnablePlatform
{
    public event Action<GameObject> OnRequestDespawn;

    public bool ShouldJump { get; private set; } = true;

    public void OnPlayerLanded()
    {
        OnRequestDespawn?.Invoke(gameObject);
    }
}