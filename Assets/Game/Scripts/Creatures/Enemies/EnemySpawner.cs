using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemySpawnSettings> _enemySpawnSettings;

    [SerializeField] private float _offsetFromScreenBounds = 2.0f;
    [SerializeField] private float _offsetEnemySpawnsOffScreen = 2.0f;
    [SerializeField] private float _minVerticalEnemyDistance = 2.0f;
    [SerializeField] private float _enemySpawnInterval = 5.0f;
    [SerializeField] private int _maxEnemyActiveAtTheSameTime = 3;

    private Dictionary<EnemySpawnSettings, Queue<GameObject>> _enemiesPools = new Dictionary<EnemySpawnSettings, Queue<GameObject>>();
    private Dictionary<GameObject, EnemySpawnSettings> _activeEnemies = new Dictionary<GameObject, EnemySpawnSettings>();

    private Camera _mainCamera;
    private float _enemySpawnTime;
    private float _lastSpawnY;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        _lastSpawnY = transform.position.y;
        _enemySpawnTime = Time.time + _enemySpawnInterval;
    }

    private void Update()
    {
        if (Time.time >= _enemySpawnTime && _activeEnemies.Count < _maxEnemyActiveAtTheSameTime)
        {
            GenerateNextEnemy();
            _enemySpawnTime = Time.time + _enemySpawnInterval;
        }

        DespawnBelowScreen();
    }

    public void GenerateNextEnemy()
    {
        var availableSettings = _enemySpawnSettings;

        EnemySpawnSettings chosenSettings = GetRandomSettingsByWeight(availableSettings);

        float nextEnemyY = _mainCamera.transform.position.y + GetCameraHalfHeight() + _offsetEnemySpawnsOffScreen;

        if (nextEnemyY > _lastSpawnY + _minVerticalEnemyDistance)
        {
            Debug.Log("Enemy spawned! _lastSpawnY = nextEnemyY;");
            _lastSpawnY = nextEnemyY;
        }
        else
        {
            Debug.Log("Enemy spawned! _lastSpawnY += _lastSpawnY + _minVerticalEnemyDistance;");
            _lastSpawnY = _lastSpawnY + _minVerticalEnemyDistance;
        }

        SpawnEnemy(chosenSettings, _lastSpawnY);
    }

    private void SpawnEnemy(EnemySpawnSettings chosenSettings, float nextSpawnY)
    {
        GameObject enemy;

        if (_enemiesPools.TryGetValue(chosenSettings, out var queue) && queue.Count > 0)
        {
            enemy = queue.Dequeue();
        }
        else
        {
            enemy = Instantiate(chosenSettings.Prefab);
        }

        float cameraLeft = _mainCamera.transform.position.x - GetCameraHalfWidth();
        float cameraRight = _mainCamera.transform.position.x + GetCameraHalfWidth();

        float targetX = Random.Range(cameraLeft + _offsetFromScreenBounds, cameraRight - _offsetFromScreenBounds);

        enemy.transform.position = new Vector3(targetX, nextSpawnY, 0f);
        enemy.SetActive(true);

        _activeEnemies.Add(enemy, chosenSettings);
    }

    private EnemySpawnSettings GetRandomSettingsByWeight(List<EnemySpawnSettings> availableSettings)
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
        var enemiesToCheck = new List<GameObject>(_activeEnemies.Keys);

        for (int i = enemiesToCheck.Count - 1; i >= 0; i--)
        {
            GameObject enemy = enemiesToCheck[i];

            if (enemy.transform.position.y < screenBottomY)
            {
                DespawnEnemy(enemy);
            }
        }
    }

    public void DespawnEnemy(GameObject enemy)
    {
        if (_activeEnemies.TryGetValue(enemy, out var originalSettings))
        {
            enemy.SetActive(false);

            if (_enemiesPools.TryGetValue(originalSettings, out var queue))
            {
                queue.Enqueue(enemy);
            }
            else
            {
                var newQueue = new Queue<GameObject>();
                newQueue.Enqueue(enemy);
                _enemiesPools.Add(originalSettings, newQueue);
            }

            _activeEnemies.Remove(enemy);
        }
        else
        {
            Debug.LogWarning($"Attempting to despawn a platform {enemy.name} that is not in the active list!");
        }
    }

    private float GetCameraHalfWidth()
    {
        return _mainCamera.orthographicSize * _mainCamera.aspect;
    }

    private float GetCameraHalfHeight()
    {
        return _mainCamera.orthographicSize;
    }
}
