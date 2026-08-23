using UnityEngine;

public interface IPlatformBehavior
{
    bool ShouldJump { get; }
    void OnPlayerLanded();
}
