using System;
using UnityEngine;

public interface IDespawnablePlatform
{
    event Action<GameObject> OnRequestDespawn;
}
