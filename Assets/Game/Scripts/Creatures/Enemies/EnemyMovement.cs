using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Collider2D _hitboxCollider;
    [SerializeField] private float _speed = 1.0f;

    private Camera _mainCamera;
    private int _direction = 1;

    public float CurrentSpeed => _direction * _speed;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        transform.position += Vector3.right * _direction * _speed * Time.fixedDeltaTime;

        if (IsBeyondHorizontalBounds())
        {
            _direction *= -1;
        }

        UpdateFacing();
    }

    private bool IsBeyondHorizontalBounds()
    {
        float cameraLeft = _mainCamera.transform.position.x - GetCameraHalfWidth();
        float cameraRight = _mainCamera.transform.position.x + GetCameraHalfWidth();

        bool touchingRight = _hitboxCollider.bounds.max.x >= cameraRight && _direction > 0;
        bool touchingLeft = _hitboxCollider.bounds.min.x <= cameraLeft && _direction < 0;

        return touchingRight || touchingLeft;
    }

    private float GetCameraHalfWidth()
    {
        return _mainCamera.orthographicSize * _mainCamera.aspect;
    }

    private void UpdateFacing()
    {
        transform.localScale = new Vector3(
            Mathf.Abs(transform.localScale.x) * _direction,
            transform.localScale.y,
            transform.localScale.z
        );
    }
}
