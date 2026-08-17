using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private GameInputActions _gameInputActions;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _gameInputActions = new GameInputActions();
        _gameInputActions.Enable();
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = _gameInputActions.Player.Move.ReadValue<Vector2>();

        return inputVector;
    }
}
