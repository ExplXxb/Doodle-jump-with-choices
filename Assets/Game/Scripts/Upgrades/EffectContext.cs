public class EffectContext
{
    public PlayerStats Stats { get; }
    public GameSettings Settings { get; }

    public EffectContext(PlayerStats stats, GameSettings settings)
    {
        Stats = stats;
        Settings = settings;
    }
}
