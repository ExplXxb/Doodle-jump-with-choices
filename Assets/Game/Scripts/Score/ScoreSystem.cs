using System;
using UnityEngine;
using VContainer;

public class ScoreSystem : MonoBehaviour
{
    public event Action<int> OnScoreChanged;

    [Header("General")]
    [SerializeField] private float _heightMultiplier = 10.0f;

    private int _score;

    private Player _player;

    [Inject]
    public void Construct(Player player)
    {
        _player = player;
    }

    public int Score
    {
        get { return _score; }
        private set
        {
            if (Score != value)
            {
                _score = value;

                OnScoreChanged?.Invoke(Score);
            }

        }
    }

    private float _playerMaxHeight;

    private void Start()
    {
        _player.OnMaxHeightChanged += Player_OnMaxHeightChanged;
    }

    private void OnDestroy()
    {
        if (_player != null)
            _player.OnMaxHeightChanged -= Player_OnMaxHeightChanged;
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
