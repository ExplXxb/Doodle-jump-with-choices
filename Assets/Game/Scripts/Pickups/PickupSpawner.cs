using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PickupSpawnOption
{
    public GameObject Prefab;
    public float BaseWeight = 1f;
    [System.NonSerialized] public float WeightModifier = 0f;

    public float EffectiveWeight => Mathf.Max(0f, BaseWeight + WeightModifier);
}

public class PickupSpawner : MonoBehaviour
{
    public static PickupSpawner Instance { get; private set; }

    [SerializeField] private Camera _mainCamera;

    [SerializeField] private List<PickupSpawnOption> _pickupOptions;
    [SerializeField] private float _targetPickupsPerScreen = 1.2f;
    [SerializeField] private Vector3 _localSpawnOffset = new Vector3(0f, 2.5f, 0f);

    private static float _chanceMultiplier = 1f;

    private Dictionary<GameObject, Queue<GameObject>> _pools = new Dictionary<GameObject, Queue<GameObject>>();
    private Dictionary<GameObject, (GameObject pickup, GameObject prefab)> _activeByPlatform
        = new Dictionary<GameObject, (GameObject, GameObject)>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public static void AddChanceModifier(float delta) => _chanceMultiplier += delta;

    public void TrySpawnPickupOnPlatform(GameObject platform)
    {
        float chance = GetChancePerPlatform();

        if (_pickupOptions == null || _pickupOptions.Count == 0 || Random.value > chance)
            return;

        var chosenOption = GetRandomOptionByWeight(_pickupOptions);
        if (chosenOption?.Prefab == null)
            return;

        GameObject pickup = GetPooledPickup(chosenOption.Prefab);
        pickup.transform.SetParent(platform.transform);
        pickup.transform.localPosition = _localSpawnOffset;
        pickup.SetActive(true);

        _activeByPlatform[platform] = (pickup, chosenOption.Prefab);
    }

    public void DespawnPickupForPlatform(GameObject platform)
    {
        if (!_activeByPlatform.TryGetValue(platform, out var entry))
            return;

        entry.pickup.transform.SetParent(transform);
        entry.pickup.SetActive(false);

        if (!_pools.TryGetValue(entry.prefab, out var queue))
        {
            queue = new Queue<GameObject>();
            _pools[entry.prefab] = queue;
        }
        queue.Enqueue(entry.pickup);

        _activeByPlatform.Remove(platform);
    }

    private float GetChancePerPlatform()
    {
        var zone = GameSettings.Instance.CurrentGenerationSettings;
        float avgDistance = (zone.MinVerticalPlatformDistance + zone.MaxVerticalPlatformDistance) / 2f;

        float bottomY = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        float topY = _mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        float screenHeight = topY - bottomY;

        float platformsPerScreen = screenHeight / Mathf.Max(avgDistance, 0.0001f);
        float chance = _targetPickupsPerScreen / Mathf.Max(platformsPerScreen, 0.0001f);

        return Mathf.Clamp01(chance * _chanceMultiplier);
    }

    private GameObject GetPooledPickup(GameObject prefab)
    {
        if (_pools.TryGetValue(prefab, out var queue) && queue.Count > 0)
            return queue.Dequeue();

        return Instantiate(prefab);
    }

    private PickupSpawnOption GetRandomOptionByWeight(List<PickupSpawnOption> options)
    {
        float totalWeight = 0f;
        foreach (var o in options) totalWeight += o.EffectiveWeight;

        float randomValue = Random.Range(0, totalWeight);
        float currentWeightSum = 0f;

        foreach (var o in options)
        {
            currentWeightSum += o.EffectiveWeight;
            if (randomValue <= currentWeightSum)
                return o;
        }
        return options[0];
    }
}
