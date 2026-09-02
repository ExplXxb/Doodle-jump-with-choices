using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Generation Zone")]
public class GenerationSettings : ScriptableObject
{
    [Min(0.01f)]
    [SerializeField] private float _minVerticalPlatformDistance;
    [Min(0.01f)]
    [SerializeField] private float _maxVerticalPlatformDistance;

    [Min(0.01f)]
    [SerializeField] private float _minHorizontalPlatformDistance;
    [Min(0.01f)]
    [SerializeField] private float _maxHorizontalPlatformDistance;

    [SerializeField] private List<PlatformSpawnSettings> _platforms;

    public float MinVerticalPlatformDistance => _minVerticalPlatformDistance;
    public float MaxVerticalPlatformDistance => _maxVerticalPlatformDistance;
    public float MinHorizontalPlatformDistance => _minHorizontalPlatformDistance;
    public float MaxHorizontalPlatformDistance => _maxHorizontalPlatformDistance;
    public List<PlatformSpawnSettings> Platforms => _platforms;


    private void OnValidate()
    {

        if (_maxVerticalPlatformDistance < _minVerticalPlatformDistance)
            _maxVerticalPlatformDistance = _minVerticalPlatformDistance;

        if (_maxHorizontalPlatformDistance < _minHorizontalPlatformDistance)
            _maxHorizontalPlatformDistance = _minHorizontalPlatformDistance;
    }

    public PlatformSpawnSettings GetPlatformSettingsByPrefab(GameObject platformPrefab)
    {
        foreach (var platformSettings in Platforms)
        {
            if (platformSettings.Prefab == platformPrefab)
            {
                return platformSettings;
            }
        }

        return null;
    }

    public void TryAddPlatformWeight(PlatformSpawnSettings settings, float addWeight)
    {
        PlatformSpawnSettings existing = GetPlatformSettingsByPrefab(settings.Prefab);
        if (existing != null)
        {
            existing.Weight += addWeight;
            return;
        }
        _platforms.Add(settings.CloneWithWeight(addWeight));
    }

    public void TryRemovePlatformWeight(GameObject platformPrefab, float removeWeight)
    {
        PlatformSpawnSettings existing = GetPlatformSettingsByPrefab(platformPrefab);
        if (existing == null)
        {
            Debug.Log("Платформу для зменшення ваги не знайдено в списку.");
            return;
        }

        if (existing.Weight - removeWeight > 0f)
        {
            existing.Weight -= removeWeight;
        }
        else
        {
            Debug.Log($"Вага платформи стала б меншою за 0 на {removeWeight - existing.Weight}, тому платформу видалено.");
            _platforms.Remove(existing);
        }
    }

    public GenerationSettings Clone()
    {
        GenerationSettings copyGenerationZone = ScriptableObject.CreateInstance<GenerationSettings>();

        copyGenerationZone._minVerticalPlatformDistance = this._minVerticalPlatformDistance;
        copyGenerationZone._maxVerticalPlatformDistance = this._maxVerticalPlatformDistance;

        copyGenerationZone._minHorizontalPlatformDistance = this._minHorizontalPlatformDistance;
        copyGenerationZone._maxHorizontalPlatformDistance = this._maxHorizontalPlatformDistance;

        copyGenerationZone._platforms = new List<PlatformSpawnSettings>();

        foreach (var platform in this._platforms)
        {
            if (platform != null)
            {
                copyGenerationZone._platforms.Add(platform.Clone());
            }
        }

        return copyGenerationZone;
    }
}