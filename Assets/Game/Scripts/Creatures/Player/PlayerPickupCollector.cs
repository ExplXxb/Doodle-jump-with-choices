using UnityEngine;

public class PlayerPickupCollector : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        if ( _player == null )
            Debug.LogError($"{nameof(PlayerPickupCollector)}: відсутній {nameof(Player)} на {name}", this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<PickupTrigger>(out var pickupHitbox))
            return;

        var pickup = other.GetComponentInParent<Pickup>();

        if (pickup == null)
        {
            Debug.LogError($"{nameof(PlayerPickupCollector)}: відсутній наслідувач класу {nameof(Pickup)} на батьківському об'єкті", this);
            return;
        }

        if (!pickup.CanPickup(_player))
            return;

        pickup.OnPickup(_player);
    }
}