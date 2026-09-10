using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    public static event System.Action<GameObject> OnAnyPickupCollected;

    [SerializeField] private PickupDirection _pickupDirection;

    public bool CanPickup(Player player)
    {
        if (player.IsFlying)
            return false;

        return _pickupDirection == PickupDirection.Any || player.IsFalling;
    }

    public void Collect(Player player)
    {
        OnPickup(player);
        OnAnyPickupCollected?.Invoke(gameObject);
    }

    public abstract void OnPickup(Player player);
}
