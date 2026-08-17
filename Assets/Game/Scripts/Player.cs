using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private float _jumpPower = 10.0f;
    [SerializeField] private float _movementSpeed = 4f;
    [SerializeField] private float _gravityAcceleration = 9.81f;
    [SerializeField] private float _maxFallSpeed = 20f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask _groundLayer;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _inputVector;
    private float _verticalSpeed;
    private bool _isGrounded;

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
    }

    private void CheckGround()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);

        if (_isGrounded && _verticalSpeed < 0f)
        {
            HandleJump();
        }
    }

    private void HandleGravity()
    {
        _verticalSpeed -= _gravityAcceleration * Time.fixedDeltaTime;
        _verticalSpeed = Mathf.Max(_verticalSpeed, -_maxFallSpeed);

        CheckGround();
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
}