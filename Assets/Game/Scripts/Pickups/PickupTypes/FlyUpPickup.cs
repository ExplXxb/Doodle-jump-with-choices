using UnityEngine;

public class FlyUpPickup : Pickup
{
    [SerializeField] private float _flySpeedMultiplier = 0.7f;
    [SerializeField] private float _flyTime = 5f;
    [SerializeField] private PickupSFX _pickupSFX;

    private void Awake()
    {
        if (_pickupSFX == null)
        {
            Debug.LogWarning($"{nameof(FlyUpPickup)}: відсутній {nameof(PickupSFX)} на {name}", this);
            _pickupSFX = GetComponentInChildren<PickupSFX>();
        }
    }

    public override void OnPickup(Player player)
    {
        player.PerformFly(_flyTime, _flySpeedMultiplier);

        _pickupSFX.PlaySound();
    }
}
