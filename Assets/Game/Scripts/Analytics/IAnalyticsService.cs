public interface IAnalyticsService
{
    void Initialize();
    void LogEvent(string eventName);
    void LogCardSpawned(UpgradeCard card);
    void LogCardSelected(UpgradeCard card);
}
