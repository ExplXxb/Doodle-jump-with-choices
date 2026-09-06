using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [Header("Prefabs")]
    [SerializeField] private Player _playerPrefab;

    [Header("Configs")]
    [SerializeField] private GenerationSettings _baseGenerationSettings;
    [SerializeField] private List<EffectSO> _allGameEffects;

    [Header("Scene Components")]
    [SerializeField] private GameplayEntryPoint _gameplayEntryPoint;
    [SerializeField] private ScoreHolder _scoreHolderUI;
    [SerializeField] private UpgradeFlowController _upgradeFlowController;
    [SerializeField] private PickupSpawner _pickupSpawner;
    [SerializeField] private PlatformSpawner _platformSpawner;
    [SerializeField] private CameraTarget _cameraTarget;
    [SerializeField] private UpgradeTrigger _upgradeTrigger;
    [SerializeField] private DeathUIController _deathUIController;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<GameInput>(Lifetime.Scoped);
        builder.RegisterEntryPoint<TimeSettingsInitializer>();
        builder.RegisterInstance(_playerPrefab);

        builder.Register<PlayerProvider>(Lifetime.Scoped).AsSelf();

        builder.Register<DefaultModeScoreSystem>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
        builder.Register<PlayerStats>(Lifetime.Scoped).AsSelf();
        builder.Register<GameSettings>(Lifetime.Scoped).WithParameter(_baseGenerationSettings).AsSelf();
        builder.Register<UpgradeOptionProvider>(Lifetime.Scoped).WithParameter(_allGameEffects);
        builder.Register<UpgradeSelectionSystem>(Lifetime.Scoped);

        builder.RegisterComponent(_scoreHolderUI);
        builder.RegisterComponent(_upgradeFlowController);
        builder.RegisterComponent(_pickupSpawner);
        builder.RegisterComponent(_platformSpawner);
        builder.RegisterComponent(_cameraTarget);
        builder.RegisterComponent(_upgradeTrigger);
        builder.RegisterComponent(_deathUIController);

        builder.RegisterComponent(_gameplayEntryPoint);
    }
}
