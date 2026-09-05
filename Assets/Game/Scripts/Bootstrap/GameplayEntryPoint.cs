using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

public class GameplayEntryPoint : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform _spawnPoint;

    [Header("Scene Systems")]
    [SerializeField] private CameraTarget _cameraTarget;
    [SerializeField] private UpgradeTrigger _upgradeTrigger;
    [SerializeField] private DeathUIController _deathUIController;

    private IObjectResolver _container;
    private Player _playerPrefab;
    private IScoreSystem _scoreSystem;

    [Inject]
    public void Construct(IObjectResolver container, Player playerPrefab, IScoreSystem scoreSystem)
    {
        _container = container;
        _playerPrefab = playerPrefab;
        _scoreSystem = scoreSystem;
    }

    private void Start()
    {
        Debug.Log("Усі сервіси геймплею ініціалізовано!");

        Vector3 spawnPosition = _spawnPoint != null ? _spawnPoint.position : Vector3.zero;

        Player spawnedPlayer = _container.Instantiate(_playerPrefab, spawnPosition, Quaternion.identity);

        if (_cameraTarget != null) _cameraTarget.Construct(spawnedPlayer);
        if (_upgradeTrigger != null) _upgradeTrigger.Construct(spawnedPlayer);
        if (_deathUIController != null) _deathUIController.Construct(spawnedPlayer);

        Debug.Log($"Гравець успішно заспавнений у точці: {spawnPosition}");
    }
}
