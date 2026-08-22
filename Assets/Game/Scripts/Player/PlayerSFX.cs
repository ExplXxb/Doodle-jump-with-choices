using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [System.Serializable]
    public struct SoundEffect
    {
        public string actionName;
        public AudioClip[] clips;
        [Range(0f, 1f)] public float volume;
    }

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
        Debug.LogWarning($"Звук для действия '{actionName}' не найден!");
    }
}