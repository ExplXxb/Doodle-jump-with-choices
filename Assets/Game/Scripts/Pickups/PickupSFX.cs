using UnityEngine;

public class PickupSFX : MonoBehaviour
{
    [SerializeField] private SoundEffect _soundEffect;

    public void PlaySound()
    {
        if (_soundEffect.clips.Length == 0)
            Debug.LogWarning($"SFX {nameof(_soundEffect.clips)} for '{this}' is not found!");

        var soundEffect = _soundEffect.clips[Random.Range(0, _soundEffect.clips.Length)];

        Vector3 spawnPosition = Camera.main.transform.position;

        AudioSource.PlayClipAtPoint(soundEffect, spawnPosition, _soundEffect.volume);
    }
}