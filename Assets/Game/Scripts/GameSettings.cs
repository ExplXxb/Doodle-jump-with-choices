using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    [SerializeField] private GenerationSettings _baseGenerationSettings;

    private GenerationSettings _currentGenerationSettings;

    public GenerationSettings CurrentGenerationParametrs => _currentGenerationSettings;

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
        _currentGenerationSettings = _baseGenerationSettings;
    }

    public void ApplyCardChoice(PlatformSpawnSettings settings, float weightDelta)
    {
        if (weightDelta >= 0f)
            _currentGenerationSettings.TryAddPlatformWeight(settings, weightDelta);
        else
            _currentGenerationSettings.TryRemovePlatformWeight(settings.Prefab, -weightDelta);
    }
}