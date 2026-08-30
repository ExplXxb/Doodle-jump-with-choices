using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField] private PickupDirection _pickupDirection;

    public bool CanPickup(Player player)
    {
        if (player.IsFlying)
            return false;

        return _pickupDirection == PickupDirection.Any || player.IsFalling;
    }

    public abstract void OnPickup(Player player);
}
