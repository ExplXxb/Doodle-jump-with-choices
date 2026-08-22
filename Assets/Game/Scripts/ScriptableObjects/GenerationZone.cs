using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Generation Zone")]
public class GenerationZone : ScriptableObject
{
    [Min(0)]
    [SerializeField] private int _minScore;
    [Min(0)]
    [SerializeField] private int _maxScore;

    [Min(0.01f)]
    [SerializeField] private float _minVerticalPlatformDistance;
    [Min(0.01f)]
    [SerializeField] private float _maxVerticalPlatformDistance;

    [Min(0.01f)]
    [SerializeField] private float _minHorizontalPlatformDistance;
    [Min(0.01f)]
    [SerializeField] private float _maxHorizontalPlatformDistance;

    [SerializeField] private List<PlatformSpawnSettings> _platforms;

    public int MinScore => _minScore;
    public int MaxScore => _maxScore;
    public float MinVerticalPlatformDistance => _minVerticalPlatformDistance;
    public float MaxVerticalPlatformDistance => _maxVerticalPlatformDistance;
    public float MinHorizontalPlatformDistance => _minHorizontalPlatformDistance;
    public float MaxHorizontalPlatformDistance => _maxHorizontalPlatformDistance;
    public List<PlatformSpawnSettings> Platforms => _platforms;


    private void OnValidate()
    {
        if (_maxScore < _minScore)
            _maxScore = _minScore;

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

    public GenerationZone Clone()
    {
        GenerationZone copyGenerationZone = ScriptableObject.CreateInstance<GenerationZone>();

        copyGenerationZone._minScore = this._minScore;
        copyGenerationZone._maxScore = this._maxScore;

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