using System;

public class PlayerProvider
{
    public event Action<Player> OnPlayerSpawned;
    public Player Instance { get; private set; }

    public void SetPlayer(Player player)
    {
        Instance = player;
        OnPlayerSpawned?.Invoke(player);
    }
}
