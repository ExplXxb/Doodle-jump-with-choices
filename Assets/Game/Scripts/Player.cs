using System;
using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event Action OnStartFalling;
    public event Action OnStartJumping;
    public event Action<float> OnMaxHeightChanged;

    [SerializeField] private float _jumpPower = 10.0f;
    [SerializeField] private float _movementSpeed = 4f;
    [SerializeField] private float _gravityAcceleration = 9.81f;
    [SerializeField] private float _maxFallSpeed = 20f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask _groundLayer;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _inputVector;
    private float _verticalSpeed = 20.0f;
    private bool _isGrounded;
    private float _maxHeight = 0f;
    
    public bool IsFalling { get; private set; }
    public Vector2 GetInputVector() => _inputVector;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _rigidbody2D = GetComponent<Rigidbody2D>();
        if (_rigidbody2D == null)
        {
            Debug.LogError($"{nameof(Player)}: відсутній Rigidbody2D на {name}", this);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Update()
    {
        _inputVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate()
    {
        HandleGravity();
        HandleMovement();
        TryUpdateMaxHeight();
    }

    private void DetermineIsFalling()
    {
        bool wasFalling = IsFalling;
        IsFalling = _verticalSpeed <= 0;

        if (IsFalling && !wasFalling)
            OnStartFalling?.Invoke();
        else if (!IsFalling && wasFalling)
            OnStartJumping?.Invoke();
    }

    private void CheckGround()
    {
        Collider2D hit = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
        _isGrounded = hit != null;

        if (_isGrounded && _verticalSpeed < 0f)
        {
            if (hit.TryGetComponent<BreakablePlatform>(out _))
            {
                return;
            }
            HandleJump();
        }
    }

    private void HandleGravity()
    {
        _verticalSpeed -= _gravityAcceleration * Time.fixedDeltaTime;
        _verticalSpeed = Mathf.Max(_verticalSpeed, -_maxFallSpeed);

        CheckGround();
        DetermineIsFalling();
    }

    private void HandleMovement()
    {
        Vector2 horizontalMovement = _inputVector * _movementSpeed * Time.fixedDeltaTime;
        Vector2 verticalMovement = new Vector2(0f, _verticalSpeed * Time.fixedDeltaTime);

        _rigidbody2D.MovePosition(_rigidbody2D.position + horizontalMovement + verticalMovement);
    }

    private void HandleJump()
    {
        _verticalSpeed = _jumpPower;
    }

    private void TryUpdateMaxHeight()
    {
        if (_maxHeight < gameObject.transform.position.y)
        {
            _maxHeight = gameObject.transform.position.y;
            OnMaxHeightChanged?.Invoke(_maxHeight);
        }
    }
}