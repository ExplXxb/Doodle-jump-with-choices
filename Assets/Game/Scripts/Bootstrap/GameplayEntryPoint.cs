using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameplayEntryPoint : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform _spawnPoint;

    [Header("Scene Systems")]
    [SerializeField] private UpgradeFlowController _upgradeFlowController;

    private IObjectResolver _container;
    private Player _playerPrefab;
    private PlayerStats _playerStats;
    private GameSettings _gameSettings;
    private PlayerProvider _playerProvider;

    [Inject]
    public void Construct(
        IObjectResolver container,
        Player playerPrefab,
        PlayerStats playerStats,
        GameSettings gameSettings,
        PlayerProvider playerProvider)
    {
        _container = container;
        _playerPrefab = playerPrefab;
        _playerStats = playerStats;
        _gameSettings = gameSettings;
        _playerProvider = playerProvider;
    }

    private void Start()
    {
        Vector3 spawnPosition = _spawnPoint != null ? _spawnPoint.position : Vector3.zero;

        Player spawnedPlayer = _container.Instantiate(_playerPrefab, spawnPosition, Quaternion.identity);

        _playerProvider.SetPlayer(spawnedPlayer);

        EffectContext effectContext = new EffectContext(_playerStats, _gameSettings);

        if (_upgradeFlowController != null)
        {
            _upgradeFlowController.SetContext(effectContext);
        }

        Debug.Log("Система апгрейдів та провайдер гравця успішно запущені!");
    }
}
