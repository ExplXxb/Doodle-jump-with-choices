using System;
using UnityEngine;

public class EnemyVisualEvents : MonoBehaviour
{
    public event Action OnDeathAnimationFinished;

    public void AnimationEvent_OnDeathFinished()
    {
        OnDeathAnimationFinished?.Invoke();
    }
}
