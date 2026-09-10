using UnityEngine;
using VContainer;

public class GoldPickup : Pickup
{
    [SerializeField] private int GoldAmount = 1;
    [SerializeField] private PickupSFX _pickupSFX;

    public override void OnPickup(Player player)
    {
        player.Wallet.AddGold(GoldAmount);

        _pickupSFX?.PlaySound();
    }
}
