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

        var pickups = other.GetComponentsInParent<Pickup>();

        if (pickups == null || pickups.Length == 0)
        {
            Debug.LogError($"{nameof(PlayerPickupCollector)}: відсутні наслідувачі класу {nameof(Pickup)} на батьківському об'єкті", this);
            return;
        }

        foreach (var pickup in pickups)
        {
            if (!pickup.CanPickup(_player))
                continue;

            pickup.Collect(_player);
        }
    }
}