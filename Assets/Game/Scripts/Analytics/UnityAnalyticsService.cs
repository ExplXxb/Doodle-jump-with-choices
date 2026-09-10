using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UnityConsent;

public class UnityAnalyticsService : IAnalyticsService
{
    private bool _isInitialized;

    public async void Initialize()
    {
        Debug.Log("[Analytics] Ініціалізація Unity Services...");
        try
        {
            await UnityServices.InitializeAsync();

            EndUserConsent.SetConsentState(new ConsentState
            {
                AnalyticsIntent = ConsentStatus.Granted,
                AdsIntent = ConsentStatus.Denied
            });

            _isInitialized = true;
            Debug.Log("[Analytics] Unity Analytics готовий до використання!");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[Analytics] Не вдалося ініціалізувати Unity Services: {e.Message}");
        }
    }

    public void LogEvent(string eventName)
    {
        if (!_isInitialized)
        {
            Debug.LogWarning($"[Analytics] UnityAnalyticsService не проініціалізовано.");
            return;
        }

        Debug.Log($"[Analytics] UnityAnalyticsService: Відправлено подію {eventName}.");
        AnalyticsService.Instance.RecordEvent(eventName);
    }

    public void LogCardSpawned(UpgradeCard upgradeCard)
    {
        if (!_isInitialized)
        {
            Debug.LogWarning($"[Analytics] UnityAnalyticsService не проініціалізовано.");
            return;
        }

        string fullCardId = $"{upgradeCard.PositiveEffect.DisplayName}_{upgradeCard.NegativeEffect.DisplayName}";

        CustomEvent cardSpawnedvent = new CustomEvent("card_spawned");

        cardSpawnedvent.Add("card_id", fullCardId);
        cardSpawnedvent.Add("positive_effect", upgradeCard.PositiveEffect.DisplayName);
        cardSpawnedvent.Add("negative_effect", upgradeCard.NegativeEffect.DisplayName);

        AnalyticsService.Instance.RecordEvent(cardSpawnedvent);

        Debug.Log($"[Analytics] RecordEvent відправив card_spawned для: {fullCardId}");
    }

    public void LogCardSelected(UpgradeCard upgradeCard)
    {
        if (!_isInitialized) return;

        string fullCardId = $"{upgradeCard.PositiveEffect.DisplayName}_{upgradeCard.NegativeEffect.DisplayName}";

        CustomEvent cardSelectedEvent = new CustomEvent("card_selected");

        cardSelectedEvent.Add("card_id", fullCardId);
        cardSelectedEvent.Add("positive_effect", upgradeCard.PositiveEffect.DisplayName);
        cardSelectedEvent.Add("negative_effect", upgradeCard.NegativeEffect.DisplayName);

        Debug.Log("[Analytics] Unity: Відправлено upgrade_card_selected.");
        AnalyticsService.Instance.RecordEvent(cardSelectedEvent);
    }
}
