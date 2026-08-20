using System;
using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event Action OnStartFalling;
    public event Action OnStartJumping;
    public event Action<float> OnMaxHeightChanged;
    public event Action OnPlayerDied;

    [Header("Drag'n'drop")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private CapsuleCollider2D _capsuleCollider2D;

    [Header("General")]
    [SerializeField] private float _jumpPower = 10.0f;
    [SerializeField] private float _movementSpeed = 4f;
    [SerializeField] private float _gravityAcceleration = 9.81f;
    [SerializeField] private float _maxFallSpeed = 20f;

    [Header("Ground Checker")]
    [SerializeField] private float _groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask _groundLayer;

    private Rigidbody2D _rigidbody;
    private Camera _mainCamera;
    private Vector2 _inputVector;
    private float _verticalSpeed = 0.0f;
    private bool _isGrounded;
    private float _maxHeight = 0f;
    private bool _isDead = false;
    
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

        _rigidbody = GetComponent<Rigidbody2D>();
        if (_rigidbody == null)
        {
            Debug.LogError($"{nameof(Player)}: відсутній Rigidbody2D на {name}", this);
        }

        _mainCamera = Camera.main;
    }

    private void Start()
    {
        HandleJump();
    }

    private void Update()
    {
        if (_isDead) return;

        if (IsBelowCamera())
        {
            Die();
        }

        if (IsBeyondHorizontalBounds())
        {
            WrapAroundHorizontally();
        }

        _inputVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate()
    {
        if (_isDead) return;

        HandleGravity();
        HandleMovement();
        TryUpdateMaxHeight();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
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

        _rigidbody.MovePosition(_rigidbody.position + horizontalMovement + verticalMovement);
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

    private bool IsBelowCamera()
    {
        float bottomCameraY = GetCameraBottomY();

        return _capsuleCollider2D.bounds.max.y < bottomCameraY;
    }

    private float GetCameraBottomY()
    {
        return _mainCamera.transform.position.y - _mainCamera.orthographicSize;
    }

    private void WrapAroundHorizontally()
    {
        float newX = transform.position.x > _mainCamera.transform.position.x
            ? _mainCamera.transform.position.x - GetCameraHalfWidth()
            : _mainCamera.transform.position.x + GetCameraHalfWidth();

        _rigidbody.position = new Vector2(newX, _rigidbody.position.y);
    }

    private bool IsBeyondHorizontalBounds()
    {
        float relativeX = transform.position.x - _mainCamera.transform.position.x;
        return Mathf.Abs(relativeX) > GetCameraHalfWidth();
    }

    private float GetCameraHalfWidth()
    {
        return _mainCamera.orthographicSize * _mainCamera.aspect;
    }

    private void Die()
    {
        if (_isDead) return;
        _isDead = true;
        Debug.Log("Гравець помер!");
        OnPlayerDied?.Invoke();
    }
}