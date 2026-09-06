using System;
using UnityEngine;
using VContainer;

public class FakePlatform : MonoBehaviour, IPlatformBehavior, IDespawnablePlatform
{
    public event Action<GameObject> OnRequestDespawn;

    public bool ShouldJump { get; private set; } = false;

    public void OnPlayerLanded()
    {
        OnRequestDespawn?.Invoke(gameObject);
    }
}
