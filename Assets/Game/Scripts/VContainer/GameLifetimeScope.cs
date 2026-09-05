using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private Player _playerPrefab;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<GameInput>(Lifetime.Scoped);

        builder.RegisterComponentInNewPrefab(_playerPrefab, Lifetime.Scoped);
    }
}
