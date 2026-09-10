using VContainer.Unity;

public class AnalyticsInitializer : IInitializable
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsInitializer(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    public void Initialize()
    {
        _analyticsService.Initialize();
    }
}
