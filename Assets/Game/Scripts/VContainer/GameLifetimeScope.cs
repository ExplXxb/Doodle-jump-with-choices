using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [Header("Prefabs")]
    [SerializeField] private Player _playerPrefab;

    [Header("Scene Entry Points")]
    [SerializeField] private GameplayEntryPoint _gameplayEntryPoint;
    [SerializeField] private ScoreHolder _scoreHolderUI;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<GameInput>(Lifetime.Scoped);
        builder.RegisterInstance(_playerPrefab);
        builder.Register<DefaultModeScoreSystem>(Lifetime.Scoped).AsImplementedInterfaces();

        builder.RegisterComponent(_scoreHolderUI);
        builder.RegisterComponent(_gameplayEntryPoint);
    }
}
