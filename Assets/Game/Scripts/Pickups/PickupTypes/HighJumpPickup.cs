using UnityEngine;

public class HighJumpPickup : Pickup
{
    [SerializeField] private float _jumpMultiplier = 2.0f;
    [SerializeField] private PickupSFX _pickupSFX;

    private void Awake()
    {
        if (_pickupSFX == null)
        {
            Debug.LogWarning($"{nameof(HighJumpPickup)}: відсутній {nameof(PickupSFX)} на {name}", this);
            _pickupSFX = GetComponentInChildren<PickupSFX>();
        }
    }

    public override void OnPickup(Player player)  
    {
        player.PerformPickupJump(_jumpMultiplier);

        _pickupSFX.PlaySound();
    }
}
