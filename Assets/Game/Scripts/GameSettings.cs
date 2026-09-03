using System.Collections.Generic;
using UnityEngine;
public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    [SerializeField] private GenerationSettings _baseGenerationSettings;

    private List<GenerationSettingsEffectSO> _activeEffects = new List<GenerationSettingsEffectSO>();
    private GenerationSettings _currentGenerationSettings;
    public GenerationSettings CurrentGenerationSettings => _currentGenerationSettings;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        RecalculateGenerationSettings();
    }

    public void AddEffect(GenerationSettingsEffectSO effect)
    {
        _activeEffects.Add(effect);
        RecalculateGenerationSettings();
    }

    public void RemoveEffect(GenerationSettingsEffectSO effect)
    {
        _activeEffects.Remove(effect);
        RecalculateGenerationSettings();
    }

    private void RecalculateGenerationSettings()
    {
        GenerationSettings result = _baseGenerationSettings.Clone();
        foreach (var effect in _activeEffects)
        {
            foreach (var settings in effect.PlatformSpawnSettings)
            {
                result.ApplyWeightDelta(settings);
            }
        }
        _currentGenerationSettings = result;
    }
}