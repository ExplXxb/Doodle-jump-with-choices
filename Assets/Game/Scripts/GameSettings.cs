using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    [SerializeField] private GenerationSettings _generationSettings;
    [SerializeField] private ScoreSystem _scoreSystem;

    private GenerationZone _currentGenerationZone;

    public GenerationZone CurrentGenerationZone
    {
        get
        {
            return _currentGenerationZone;
        }
    }

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
        _currentGenerationZone = _generationSettings.Zones[0];
        _currentGenerationZone = TryGetSuitableGenerationZone(_currentGenerationZone);
    }

    private void Update()
    {
        if (_scoreSystem.Score < _currentGenerationZone.MinScore || _scoreSystem.Score > _currentGenerationZone.MaxScore)
        {
            _currentGenerationZone = TryGetSuitableGenerationZone(_currentGenerationZone);
        }
    }

    private GenerationZone TryGetSuitableGenerationZone(GenerationZone currentGenerationZone)
    {
        int currentScore = _scoreSystem.Score;
        GenerationZone result = currentGenerationZone.Clone();

        List<GenerationZone> suitableZones = new List<GenerationZone>();

        foreach (var zone in _generationSettings.Zones)
        {
            if (currentScore >= zone.MinScore && currentScore <= zone.MaxScore)
            {
                suitableZones.Add(zone);
            }
        }

        if (suitableZones.Count > 1)
        {
            result = suitableZones[Random.Range(0, suitableZones.Count)];
        }
        else if (suitableZones.Count == 1)
        {
            result = suitableZones[0];
        }
        else
        {
            return GetMostSuitableGenerationZone();
        }

        Debug.Log(result);
        return result;
    }

    private GenerationZone GetMostSuitableGenerationZone()
    {
        int currentScore = _scoreSystem.Score;
        int minUnsuitability = int.MaxValue;

        GenerationZone result = _currentGenerationZone;

        foreach (var zone in _generationSettings.Zones)
        {
            if (currentScore < zone.MinScore)
            {
                if (minUnsuitability > zone.MinScore - currentScore)
                {
                    minUnsuitability = zone.MinScore - currentScore;
                    result = zone;
                }
            }
            if (currentScore > zone.MaxScore)
            {
                if (minUnsuitability > currentScore - zone.MaxScore)
                {
                    minUnsuitability = currentScore - zone.MaxScore;
                    result = zone;
                }
            }
        }

        return result;
    }
}