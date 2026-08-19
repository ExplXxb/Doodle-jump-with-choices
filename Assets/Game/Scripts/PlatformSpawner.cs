using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    public static PlatformSpawner Instance {  get; private set; }

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
        if (Instance == null)
            Instance = this;

        InitializePool();
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

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
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
            if (_activePlatforms[i].transform.position.y < screenBottomY)
            {
                _activePlatforms[i].SetActive(false);
                _pool.Enqueue(_activePlatforms[i]);
                _activePlatforms.RemoveAt(i);
            }
        }
    }

    public void DespawnPlatform(GameObject platform)
    {
        platform.SetActive(false);
        _pool.Enqueue(platform);
        _activePlatforms.Remove(platform);
    }
}
