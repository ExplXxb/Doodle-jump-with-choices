using UnityEngine;
using VContainer;

public class PlayerVisual : MonoBehaviour
{
    private const string IS_FALLING = "IsFalling";

    [SerializeField] private GameObject _jumpingVFXPrefab;
    [SerializeField] private Vector2 _vfxOffset = new Vector2(0f, -0.267f);

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private Player _player;

    public void Construct(Player player)
    {
        _player = player;

        _player.OnStartFalling += SetFallingAnimation;
        _player.OnStartJumping += SetJumpingAnimation;
        _player.OnStartJumping += HandleJumpingVFX;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_player == null) return;

        SetFaceDirection();
    }

    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.OnStartFalling -= SetFallingAnimation;
            _player.OnStartJumping -= SetJumpingAnimation;
            _player.OnStartJumping -= HandleJumpingVFX;
        }
    }

    private void SetFallingAnimation() => _animator.SetBool(IS_FALLING, true);
    private void SetJumpingAnimation() => _animator.SetBool(IS_FALLING, false);

    private void SetFaceDirection()
    {
        float horizontalInput = _player.GetInputVector().x;

        if (horizontalInput > 0.01f)
            _spriteRenderer.flipX = false;
        else if (horizontalInput < -0.01f)
            _spriteRenderer.flipX = true;
    }

    private void HandleJumpingVFX()
    {
        Vector2 spawnPosition = _player.GetGroundCheckPosition() + _vfxOffset;
        Instantiate(_jumpingVFXPrefab, spawnPosition, Quaternion.identity);
    }
}
