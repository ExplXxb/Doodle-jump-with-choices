using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ProjectLifetimeScope : LifetimeScope
{
    [SerializeField] private ShopConfig _shopConfig;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_shopConfig);

        builder.RegisterEntryPoint<MetaProgressService>(Lifetime.Singleton).AsSelf();
    }
}
