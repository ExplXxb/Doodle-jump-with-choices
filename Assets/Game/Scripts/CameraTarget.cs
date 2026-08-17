using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Transform _player;
    private float _highestY;

    private void Start()
    {
        _highestY = _player.position.y;
    }

    private void LateUpdate()
    {
        _highestY = Mathf.Max(_highestY, _player.position.y);
        transform.position = new Vector3(transform.position.x, _highestY, transform.position.z);
    }
}