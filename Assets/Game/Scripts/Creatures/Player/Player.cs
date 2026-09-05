using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Windows;
using VContainer;

[SelectionBase]
public class Player : MonoBehaviour
{
    private const string JUMP = "Jump";
    private const float WRAP_SAFETY_MARGIN = 0.01f;

    public static Player Instance;

    public event Action OnStartFalling;
    public event Action OnStartJumping;
    public event Action<float> OnMaxHeightChanged;

    [Header("Drag'n'drop")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private CapsuleCollider2D _capsuleCollider2D;
    [SerializeField] private PlayerSFX _playerSFX;

    [Header("General")]
    [SerializeField] private float _maxFallSpeed = 1000f;

    [Header("Ground Checker")]
    [SerializeField] private float _groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask _groundLayer;

    private Rigidbody2D _rigidbody;
    private Camera _mainCamera;
    private Vector2 _inputVector;
    private float _verticalSpeed = 0.0f;
    private bool _isGrounded;
    private float _maxHeight = 0f;
    private Health _health;
    private PlayerHitReaction _hitReaction;

    private IInput _input;

    public bool IsFalling { get; private set; }
    public bool IsFlying { get; private set; }
    public bool IsDead => _health.IsDead;
    public Vector2 GetInputVector() => _inputVector;
    public Vector2 GetGroundCheckPosition() => _groundCheck.position;

    [Inject]
    public void Construct(IInput input)
    {
        _input = input;
    }

    private void Awake()
    {
        Instance = this;

        _rigidbody = GetComponent<Rigidbody2D>();
        if (_rigidbody == null)
        {
            Debug.LogError($"{nameof(Player)}: відсутній Rigidbody2D на {name}", this);
        }

        _health = GetComponent<Health>();
        if (_health == null)
        {
            Debug.LogError($"{nameof(Player)}: відсутній {nameof(Health)} на {name}", this);
        }

        _hitReaction = GetComponent<PlayerHitReaction>();
        if (_hitReaction == null)
        {
            Debug.LogError($"{nameof(Player)}: відсутній {nameof(PlayerHitReaction)} на {name}", this);
        }

        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _health.OnTakeDamage += HandleTakeDamage;
        _health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        _health.OnTakeDamage -= HandleTakeDamage;
        _health.OnDeath -= HandleDeath;
    }

    private void Start()
    {
        _verticalSpeed = PlayerStats.Instance.JumpPower;
    }

    private void Update()
    {
        if (IsDead) return;

        if (IsBelowCamera())
        {
            _health.Kill();
        }

        if (IsBeyondHorizontalBounds())
        {
            WrapAroundHorizontally();
        }

        _inputVector = _input.GetMovementVector();
    }

    private void FixedUpdate()
    {
        if (IsDead) return;

        _hitReaction.Tick(Time.fixedDeltaTime);

        if (IsFlying == false)
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
    }

    private void CheckGround()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            _groundCheck.position,
            _groundCheckRadius,
            _groundLayer
        );

        _isGrounded = hit != null;

        if (!_isGrounded || _verticalSpeed >= 0f)
            return;

        if (hit.TryGetComponent<IPlatformBehavior>(out var platform))
        {
            platform.OnPlayerLanded();

            if (platform.ShouldJump)
            {
                HandleJump();
            }

            return;
        }

        var fallingDamageReceiver = hit.GetComponentInParent<FallingDamageReceiver>();

        if (fallingDamageReceiver != null)
        {
            fallingDamageReceiver.ReceiveFallingDamage(PlayerStats.Instance.StompDamage, transform.position);
            HandleJump();
            return;
        }

        HandleJump();
    }

    private void HandleGravity()
    {
        _verticalSpeed -= PlayerStats.Instance.GravityAcceleration * Time.fixedDeltaTime;
        _verticalSpeed = Mathf.Max(_verticalSpeed, -_maxFallSpeed);

        CheckGround();
        DetermineIsFalling();
    }

    private void HandleMovement()
    {
        Vector2 horizontalMovement = _inputVector * _hitReaction.InputMultiplier * PlayerStats.Instance.MovementSpeed * Time.fixedDeltaTime;
        Vector2 verticalMovement = new Vector2(0f, _verticalSpeed * Time.fixedDeltaTime);
        Vector2 knockback = _hitReaction.KnockbackDisplacement;

        _rigidbody.MovePosition(_rigidbody.position + horizontalMovement + verticalMovement + knockback);
    }

    private void PerformJump(float jumpMultiplier)
    {
        if (_flyingCoroutine != null)
            return;

        _verticalSpeed = PlayerStats.Instance.JumpPower * jumpMultiplier;

        OnStartJumping?.Invoke();
    }

    private Coroutine _flyingCoroutine = null;

    public void PerformFly(float flyingTime, float flyingSpeedMultiplier, AudioClip flyingSound = null, float soundVolume = 1.0f)
    {
        if (_flyingCoroutine != null)
        {
            StopCoroutine(_flyingCoroutine);
            _flyingCoroutine = null;
        }

        if (flyingSound != null)
        {
            _playerSFX.StartPlayLoopingSound(flyingSound, soundVolume);
        }

        IsFlying = true;
            
        PerformJump(flyingSpeedMultiplier);

        _flyingCoroutine = StartCoroutine(FlyingRoutine(flyingTime));
    }

    private IEnumerator FlyingRoutine(float flyingTime)
    {
        yield return new WaitForSeconds(flyingTime);

        _flyingCoroutine = null;
        IsFlying = false;

        _playerSFX.StopPlayLoopingSound();
    }

    public void PerformPickupJump(float jumpMultiplier)
    {
        PerformJump(jumpMultiplier);
    }

    private void HandleJump()
    {
        PerformJump(1.0f);
        _playerSFX.PlaySound(JUMP);
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
        ? _mainCamera.transform.position.x - GetCameraHalfWidth() + WRAP_SAFETY_MARGIN
        : _mainCamera.transform.position.x + GetCameraHalfWidth() - WRAP_SAFETY_MARGIN;

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

    private void HandleTakeDamage(DamageInfo damageInfo)
    {
        _hitReaction.ApplyHit(damageInfo.SourcePosition, transform.position);
    }

    private void HandleDeath()
    {
        if (_flyingCoroutine != null)
        {
            StopCoroutine(_flyingCoroutine);
            _flyingCoroutine = null;

            IsFlying = false;
            _playerSFX.StopPlayLoopingSound();
        }
    }
}