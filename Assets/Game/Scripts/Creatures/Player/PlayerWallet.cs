using VContainer;

public class PlayerWallet
{
    private MetaProgressService _progress;
    public int SessionGold { get; private set; }

    [Inject]
    public void Construct(MetaProgressService progress)
    {
        _progress = progress;

        SessionGold = 0;
    }

    public void AddGold(int amount)
    {
        SessionGold += amount;
        _progress.AddGold(amount);
    }
}
