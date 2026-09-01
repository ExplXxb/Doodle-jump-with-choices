using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private SoundEffect[] _soundEffects;

    public void PlaySound(string actionName)
    {
        foreach (var effect in _soundEffects)
        {
            if (effect.actionName == actionName && effect.clips.Length > 0)
            {
                AudioClip randomClip = effect.clips[Random.Range(0, effect.clips.Length)];

                _audioSource.PlayOneShot(randomClip, effect.volume);
                return;
            }
        }
        Debug.LogWarning($"Звук для дії '{actionName}' не знайдено!");
    }

    public void StartPlayLoopingSound(AudioClip audioClip, float volume)
    {
        _audioSource.loop = true;
        _audioSource.clip = audioClip;
        _audioSource.volume = volume;
        _audioSource.Play();
    }

    public void StopPlayLoopingSound()
    {
        _audioSource.Stop();
    }
}