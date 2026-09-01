using UnityEngine;

public class FlyUpPickup : Pickup
{
    [SerializeField] private float _flySpeedMultiplier = 0.7f;
    [SerializeField] private float _flyTime = 5f;
    [SerializeField] private AudioClip _flyingSound;
    [SerializeField] private float _soundVolume;


    public override void OnPickup(Player player)
    {
        player.PerformFly(_flyTime, _flySpeedMultiplier, _flyingSound, _soundVolume);
    }
}
