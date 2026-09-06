using UnityEngine;
using System.Collections.Generic;
using VContainer;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private Camera _mainCamera;

    [SerializeField] private float _screenPaddingX = 1f;
    [SerializeField] private float _screenOffsetY = 1f;

    private Dictionary<PlatformSpawnSettings, Queue<GameObject>> _platformPools = new Dictionary<PlatformSpawnSettings, Queue<GameObject>>();
    private Dictionary<GameObject, PlatformSpawnSettings> _activePlatforms = new Dictionary<GameObject, PlatformSpawnSettings>();

    private List<GameObject> _activePlatformsList = new List<GameObject>();
    private List<PlatformSpawnSettings> _reliablePlatformsCache = new List<PlatformSpawnSettings>();

    private float _lastSpawnY;
    private float _lastReliablePlatformY;
    private float _lastSpawnX;

    private GameSettings _gameSettings;
    private PickupSpawner _pickupSpawner;

    private GenerationSettings _previousGeneraionSettings;
    private GenerationSettings _currentGenerationSettings => _gameSettings.CurrentGenerationSettings;


    [Inject]
    public void Construct(GameSettings gameSettings, PickupSpawner pickupSpawner)
    {
        _gameSettings = gameSettings;
        _pickupSpawner = pickupSpawner;

        _lastSpawnY = transform.position.y;
        _lastReliablePlatformY = _lastSpawnY;

        _lastSpawnX = _mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0, 0)).x;

        UpdateReliablePlatformsCache();
    }

    private void Update()
    {
        if (_gameSettings == null) return;

        if (_currentGenerationSettings != _previousGeneraionSettings)
        {
            UpdateReliablePlatformsCache();
        }

        float screenTopY = _mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        while (_lastSpawnY < screenTopY + _screenOffsetY + _currentGenerationSettings.MaxVerticalPlatformDistance)
        {
            GenerateNextPlatform();
        }

        DespawnBelowScreen();
    }

    private void UpdateReliablePlatformsCache()
    {
        _previousGeneraionSettings = _currentGenerationSettings;
        _reliablePlatformsCache.Clear();

        if (_currentGenerationSettings == null || _currentGenerationSettings.Platforms == null) return;

        for (int i = 0; i < _currentGenerationSettings.Platforms.Count; i++)
        {
            if (_currentGenerationSettings.Platforms[i].IsReliable)
            {
                _reliablePlatformsCache.Add(_currentGenerationSettings.Platforms[i]);
            }
        }
    }

    public void GenerateNextPlatform()
    {
        var currentZone = _currentGenerationSettings;
        var availableSettings = currentZone.Platforms;
        float maxZoneDistance = currentZone.MaxVerticalPlatformDistance;

        PlatformSpawnSettings chosenSettings;

        float nextSpawnDistance = Random.Range(currentZone.MinVerticalPlatformDistance, maxZoneDistance);

        if (_lastSpawnY + nextSpawnDistance - _lastReliablePlatformY > maxZoneDistance)
        {
            _lastSpawnY = _lastReliablePlatformY + maxZoneDistance;

            chosenSettings = _reliablePlatformsCache.Count > 0
                ? GetRandomSettingsByWeight(_reliablePlatformsCache)
                : GetRandomSettingsByWeight(availableSettings);
        }
        else
        {
            _lastSpawnY += nextSpawnDistance;

            chosenSettings = GetRandomSettingsByWeight(availableSettings);
        }

        if (chosenSettings.IsReliable)
        {
            _lastReliablePlatformY = _lastSpawnY;
        }

        SpawnPlatform(chosenSettings, _lastSpawnY);
    }

    private void SpawnPlatform(PlatformSpawnSettings chosenSettings, float nextSpawnY)
    {
        GameObject platform;

        if (_platformPools.TryGetValue(chosenSettings, out var queue) && queue.Count > 0)
        {
            platform = queue.Dequeue();
        }
        else
        {
            platform = Instantiate(chosenSettings.Prefab);

            if (platform.TryGetComponent<IDespawnablePlatform>(out var despawnablePlatform))
            {
                despawnablePlatform.OnRequestDespawn += DespawnPlatform;
            }
        }

        float minXDist = _currentGenerationSettings.MinHorizontalPlatformDistance;
        float maxXDist = _currentGenerationSettings.MaxHorizontalPlatformDistance;

        float randomDistanceX = Random.Range(minXDist, maxXDist);
        float directionX = Random.value > 0.5f ? 1f : -1f;
        float targetX = _lastSpawnX + (randomDistanceX * directionX);

        float screenLeftX = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + _screenPaddingX;
        float screenRightX = _mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - _screenPaddingX;

        if (targetX > screenRightX)
        {
            targetX = _lastSpawnX - randomDistanceX;
        }
        else if (targetX < screenLeftX)
        {
            targetX = _lastSpawnX + randomDistanceX;
        }

        targetX = Mathf.Clamp(targetX, screenLeftX, screenRightX);
        _lastSpawnX = targetX;

        platform.transform.position = new Vector3(targetX, nextSpawnY, 0f);
        platform.SetActive(true);

        _activePlatforms.Add(platform, chosenSettings);
        _activePlatformsList.Add(platform);

        if (_pickupSpawner != null)
        {
            _pickupSpawner.TrySpawnPickupOnPlatform(platform);
        }
    }

    private PlatformSpawnSettings GetRandomSettingsByWeight(List<PlatformSpawnSettings> availableSettings)
    {
        if (availableSettings == null || availableSettings.Count == 0) return null;

        float totalWeight = 0;
        foreach (var settings in availableSettings) totalWeight += settings.Weight;

        float randomValue = Random.Range(0, totalWeight);
        float currentWeightSum = 0;

        foreach (var settings in availableSettings)
        {
            currentWeightSum += settings.Weight;
            if (randomValue <= currentWeightSum)
            {
                return settings;
            }
        }
        return availableSettings[0];
    }

    private void DespawnBelowScreen()
    {
        float screenBottomY = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        var platformsToCheck = new List<GameObject>(_activePlatforms.Keys);

        for (int i = _activePlatformsList.Count - 1; i >= 0; i--)
        {
            GameObject platform = _activePlatformsList[i];

            if (platform.transform.position.y < screenBottomY)
            {
                DespawnPlatform(platform);
            }
        }
    }

    public void DespawnPlatform(GameObject platform)
    {
        if (_activePlatforms.TryGetValue(platform, out var originalSettings))
        {
            _pickupSpawner.DespawnPickupForPlatform(platform);

            platform.SetActive(false);

            if (_platformPools.TryGetValue(originalSettings, out var queue))
            {
                queue.Enqueue(platform);
            }
            else
            {
                var newQueue = new Queue<GameObject>();
                newQueue.Enqueue(platform);
                _platformPools.Add(originalSettings, newQueue);
            }

            _activePlatforms.Remove(platform);
            _activePlatformsList.Remove(platform);
        }
        else
        {
            Debug.LogWarning($"Attempting to despawn a platform {platform.name} that is not in the active list!");
        }
    }
}
