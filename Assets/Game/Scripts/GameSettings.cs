using System.Collections.Generic;
using UnityEngine;
public class GameSettings
{
    private readonly GenerationSettings _baseGenerationSettings;
    private readonly List<GenerationSettingsEffectSO> _activeEffects = new List<GenerationSettingsEffectSO>();

    private GenerationSettings _currentGenerationSettings;
    public GenerationSettings CurrentGenerationSettings => _currentGenerationSettings;

    public GameSettings(GenerationSettings baseGenerationSettings)
    {
        _baseGenerationSettings = baseGenerationSettings;
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