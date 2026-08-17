using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject[] _platformPrefabs;
    [SerializeField] private float _minVerticalGap = 1f;
    [SerializeField] private float _maxVerticalGap = 2.5f;
    [SerializeField] private float _horizontalRange = 3.5f;
    [SerializeField] private int _poolSize = 20;

    private Queue<GameObject> _pool = new Queue<GameObject>();
    private List<GameObject> _activePlatforms = new List<GameObject>();
    private float _lastSpawnY;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        foreach (var prefab in _platformPrefabs)
        {
            for (int i = 0; i < _poolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                _pool.Enqueue(obj);
            }
        }
    }

    private void Update()
    {
        float screenTopY = _mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        while (_lastSpawnY < screenTopY + _maxVerticalGap)
        {
            SpawnPlatform();
        }

        DespawnBelowScreen();
    }

    private void SpawnPlatform()
    {
        if (_pool.Count == 0) return;

        GameObject platform = _pool.Dequeue();
        float randomX = Random.Range(-_horizontalRange, _horizontalRange);
        float randomGap = Random.Range(_minVerticalGap, _maxVerticalGap);

        _lastSpawnY += randomGap;
        platform.transform.position = new Vector3(randomX, _lastSpawnY, 0);
        platform.SetActive(true);
        _activePlatforms.Add(platform);
    }

    private void DespawnBelowScreen()
    {
        float screenBottomY = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;

        for (int i = _activePlatforms.Count - 1; i >= 0; i--)
        {
            if (_activePlatforms[i].transform.position.y < screenBottomY - 1f)
            {
                _activePlatforms[i].SetActive(false);
                _pool.Enqueue(_activePlatforms[i]);
                _activePlatforms.RemoveAt(i);
            }
        }
    }
}
