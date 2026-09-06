using System;

public class DefaultModeScoreSystem : IDisposable, IScoreSystem
{
    public event Action<int> OnScoreChanged;

    private readonly float _heightMultiplier = 5.0f;
    private int _score;
    private Player _player;
    private float _playerMaxHeight;

    public DefaultModeScoreSystem(PlayerProvider playerProvider)
    {
        if (playerProvider.Instance != null)
            BindPlayer(playerProvider.Instance);

        playerProvider.OnPlayerSpawned += BindPlayer;
    }

    private void BindPlayer(Player player)
    {
        _player = player;
        _player.OnMaxHeightChanged += Player_OnMaxHeightChanged;
    }


    public int Score
    {
        get { return _score; }
        private set
        {
            if (_score != value)
            {
                _score = value;

                OnScoreChanged?.Invoke(Score);
            }
        }
    }

    void IDisposable.Dispose()
    {
        if (_player != null)
        {
            _player.OnMaxHeightChanged -= Player_OnMaxHeightChanged;
        }
    }

    private void Player_OnMaxHeightChanged(float newPlayerMaxHeight)
    {
        _playerMaxHeight = newPlayerMaxHeight;
        CalculateNewScore();
    }

    private void CalculateNewScore()
    {
        Score = (int)(_playerMaxHeight * _heightMultiplier);
    }
}
