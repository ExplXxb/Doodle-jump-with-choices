using UnityEngine;
using VContainer;

public class CameraTarget : MonoBehaviour
{
    private float _highestY;
    private Transform _playerTransform;

    [Inject]
    public void Construct(PlayerProvider playerProvider)
    {
        if (playerProvider.Instance != null)
            InitCamera(playerProvider.Instance);

        playerProvider.OnPlayerSpawned += InitCamera;
    }

    private void InitCamera(Player player)
    {
        _playerTransform = player.transform;
        _highestY = _playerTransform.position.y;
    }

    private void LateUpdate()
    {
        if (_playerTransform == null) return;

        _highestY = Mathf.Max(_highestY, _playerTransform.position.y);
        transform.position = new Vector3(transform.position.x, _highestY, transform.position.z);
    }
}