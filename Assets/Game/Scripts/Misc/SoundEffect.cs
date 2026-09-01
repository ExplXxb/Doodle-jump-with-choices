using UnityEngine;

[System.Serializable]
public struct SoundEffect
{
    public string actionName;
    public AudioClip[] clips;
    [Range(0f, 1f)] public float volume;
}