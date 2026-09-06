using System;
using UnityEngine;
using VContainer.Unity;

public class GameInput : IDisposable, IInput
{
    private readonly GameInputActions _gameInputActions;

    public GameInput()
    {
        _gameInputActions = new GameInputActions();
        _gameInputActions.Enable();
    }

    void IDisposable.Dispose()
    {
        _gameInputActions?.Disable();
        _gameInputActions?.Dispose();
    }

    public Vector2 GetMovementVector() => _gameInputActions.Player.Move.ReadValue<Vector2>();
}
