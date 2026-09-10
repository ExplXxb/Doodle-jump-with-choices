using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ProjectLifetimeScope : LifetimeScope
{
    [SerializeField] private ShopConfig _shopConfig;

    protected override void Configure(IContainerBuilder builder)
    {
#if UNITY_WEBGL
        builder.Register<IAnalyticsService, UnityAnalyticsService>(Lifetime.Singleton);
#else
        builder.Register<IAnalyticsService, FirebaseAnalyticsService>(Lifetime.Singleton);
#endif

        _shopConfig.Initialize();
        builder.RegisterInstance(_shopConfig).AsSelf();
        builder.RegisterEntryPoint<MetaProgressService>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<AnalyticsInitializer>();
    }
}
