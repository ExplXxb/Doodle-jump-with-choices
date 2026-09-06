using System;

public interface IScoreSystem
{
    event Action<int> OnScoreChanged;

    int Score { get; }
}
