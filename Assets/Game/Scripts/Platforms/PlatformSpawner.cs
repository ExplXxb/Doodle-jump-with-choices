using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    public static PlatformSpawner Instance { get; private set; }

    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private Camera _mainCamera;

    [SerializeField] private float _screenPaddingX = 1f;

    private Dictionary<GameObject, Queue<GameObject>> _platformPools = new Dictionary<GameObject, Queue<GameObject>>();
    private Dictionary<GameObject, PlatformSpawnSettings> _activePlatforms = new Dictionary<GameObject, PlatformSpawnSettings>();

    private float _lastSpawnY;
    private float _lastReliablePlatformY;
    private float _lastSpawnX;

    private GenerationZone _сurrentGenerationZone => GameSettings.Instance.CurrentGenerationZone;

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
    }

    private void Update()
    {
        float screenTopY = _mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        while (_lastSpawnY < screenTopY + _сurrentGenerationZone.MaxVerticalPlatformDistance)
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

            var reliableOptions = availableSettings.FindAll(s => s.IsReliable);

            chosenSettings = reliableOptions.Count > 0
                ? GetRandomSettingsByWeight(reliableOptions)
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

        if (_platformPools.TryGetValue(chosenSettings.Prefab, out var queue) && queue.Count > 0)
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

        for (int i = platformsToCheck.Count - 1; i >= 0; i--)
        {
            GameObject platform = platformsToCheck[i];

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
            platform.SetActive(false);

            if (_platformPools.TryGetValue(originalSettings.Prefab, out var queue))
            {
                queue.Enqueue(platform);
            }
            else
            {
                var newQueue = new Queue<GameObject>();
                newQueue.Enqueue(platform);
                _platformPools.Add(originalSettings.Prefab, newQueue);
            }

            _activePlatforms.Remove(platform);
        }
        else
        {
            Debug.LogWarning($"Attempting to despawn a platform {platform.name} that is not in the active list!");
        }
    }
}
