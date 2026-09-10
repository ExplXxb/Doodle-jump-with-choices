#if !UNITY_WEBGL


using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;

public class FirebaseAnalyticsService : IAnalyticsService
{
    private bool _isInitialized;

    private FirebaseApp _app;

    public void Initialize()
    {
        Debug.Log("[Analytics] Перевірка залежностей Firebase для android...");

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {

                _app = FirebaseApp.DefaultInstance;

                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

                _isInitialized = true;
                Debug.Log("[Analytics] Firebase готовий до використання на android!");

            }
            else
            {
                Debug.LogError(string.Format(
                    "[Analytics] Не вдалося вирішити всі залежності Firebase: {0}", dependencyStatus));
            }
        });
    }

    public void LogEvent(string eventName)
    {
        if (!_isInitialized)
        {
            Debug.LogWarning($"[Analytics] FirebaseAnalyticsService не проініціалізовано.");
            return;
        }

        Debug.Log($"[Analytics] Виконано LogEvent й відправлено {eventName} на сервер (PC/android).");
        FirebaseAnalytics.LogEvent(eventName);
    }

    public void LogCardSpawned(UpgradeCard upgradeCard)
    {
        if (!_isInitialized)
        {
            Debug.LogWarning($"[Analytics] FirebaseAnalyticsService не проініціалізовано.");
            return;
        }

        string fullCardId = $"{upgradeCard.PositiveEffect.DisplayName}_{upgradeCard.NegativeEffect.DisplayName}";

        


        Parameter[] choiceParamaters = new Parameter[]
        {
            new Parameter("card_id", fullCardId),
            new Parameter("positive_effect", upgradeCard.PositiveEffect.DisplayName),
            new Parameter("negative_effect", upgradeCard.NegativeEffect.DisplayName)
        };

        Debug.Log("[Analytics] Виконано LogCardSpawned й відправлено upgrade_card_spawned на сервер (PC/android).");
        FirebaseAnalytics.LogEvent("upgrade_card_spawned", choiceParamaters);
    }

    public void LogCardSelected(UpgradeCard upgradeCard)
    {
        if (!_isInitialized)
        {
            Debug.LogWarning($"[Analytics] FirebaseAnalyticsService не проініціалізовано.");
            return;
        }

        string fullCardId = $"{upgradeCard.PositiveEffect.DisplayName}_{upgradeCard.NegativeEffect.DisplayName}";

        Parameter[] choiceParamaters = new Parameter[]
        {
            new Parameter("card_id", fullCardId),
            new Parameter("positive_effect", upgradeCard.PositiveEffect.DisplayName),
            new Parameter("negative_effect", upgradeCard.NegativeEffect.DisplayName)
        };

        Debug.Log("[Analytics] Виконано LogCardSelected й відправлено upgrade_card_selected на сервер (PC/android).");
        FirebaseAnalytics.LogEvent("upgrade_card_selected", choiceParamaters);
    }
}

#endif