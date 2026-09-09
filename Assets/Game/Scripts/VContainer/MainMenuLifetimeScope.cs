using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MainMenuLifetimeScope : LifetimeScope
{
    [SerializeField] private MoneyUI _moneyUI;
    [SerializeField] private ShopController _shopController;
    [SerializeField] private MainMenuController _mainMenuController;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<TimeSettingsInitializer>();

        builder.RegisterComponent(_moneyUI);
        builder.RegisterComponent(_shopController);
        builder.RegisterComponent(_mainMenuController);
    }
}
