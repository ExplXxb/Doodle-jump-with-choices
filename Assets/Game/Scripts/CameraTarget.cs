using UnityEngine;
using VContainer;

public class CameraTarget : MonoBehaviour
{
    private float _highestY;

    private Transform _playerTransform;

    public void Construct(Player player)
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