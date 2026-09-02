using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    public static PlatformSpawner Instance { get; private set; }

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

    private GenerationSettings _сurrentGenerationZone => GameSettings.Instance.CurrentGenerationParametrs;
    private GenerationSettings _previousZone;

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
        _lastSpawnY = transform.position.y;
        _lastReliablePlatformY = _lastSpawnY;

        _lastSpawnX = _mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0, 0)).x;

        UpdateReliablePlatformsCache();
    }

    private void Update()
    {
        if (_сurrentGenerationZone != _previousZone)
        {
            UpdateReliablePlatformsCache();
        }


        float screenTopY = _mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        while (_lastSpawnY < screenTopY + _screenOffsetY + _сurrentGenerationZone.MaxVerticalPlatformDistance)
        {
            GenerateNextPlatform();
        }

        DespawnBelowScreen();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void UpdateReliablePlatformsCache()
    {
        _previousZone = _сurrentGenerationZone;
        _reliablePlatformsCache.Clear();

        if (_сurrentGenerationZone == null || _сurrentGenerationZone.Platforms == null) return;

        for (int i = 0; i < _сurrentGenerationZone.Platforms.Count; i++)
        {
            if (_сurrentGenerationZone.Platforms[i].IsReliable)
            {
                _reliablePlatformsCache.Add(_сurrentGenerationZone.Platforms[i]);
            }
        }
    }

    public void GenerateNextPlatform()
    {
        var currentZone = _сurrentGenerationZone;
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
        }

        float minXDist = _сurrentGenerationZone.MinHorizontalPlatformDistance;
        float maxXDist = _сurrentGenerationZone.MaxHorizontalPlatformDistance;

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

        PickupSpawner.Instance.TrySpawnPickupOnPlatform(platform);
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
            PickupSpawner.Instance.DespawnPickupForPlatform(platform);

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
