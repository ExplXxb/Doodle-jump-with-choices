using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private GameObject _jumpingVFXPrefab;
    [SerializeField] private Vector2 _vfxOffset = new Vector2(0f, -0.267f);

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private const string IS_FALLING = "IsFalling";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Player.Instance.OnStartFalling += SetFallingAnimation;
        Player.Instance.OnStartJumping += SetJumpingAnimation;
        Player.Instance.OnStartJumping += HandleJumpingVFX;
    }

    private void Update()
    {
        SetFaceDirection();
    }

    private void OnDestroy()
    {
        if (Player.Instance != null)
        {
            Player.Instance.OnStartFalling -= SetFallingAnimation;
            Player.Instance.OnStartJumping -= SetJumpingAnimation;
            Player.Instance.OnStartJumping -= HandleJumpingVFX;
        }
    }

    private void SetFallingAnimation()
    {
        _animator.SetBool(IS_FALLING, true);
    }

    private void SetJumpingAnimation()
    {
        _animator.SetBool(IS_FALLING, false);
    }

    private void SetFaceDirection()
    {
        float horizontalInput = Player.Instance.GetInputVector().x;

        if (horizontalInput > 0.01f)
            _spriteRenderer.flipX = false;
        else if (horizontalInput < -0.01f)
            _spriteRenderer.flipX = true;
    }

    private void HandleJumpingVFX()
    {
        Vector2 spawnPosition = Player.Instance.GetGroundCheckPosition() + _vfxOffset;

        Instantiate(_jumpingVFXPrefab, spawnPosition, Quaternion.identity);
    }
}
