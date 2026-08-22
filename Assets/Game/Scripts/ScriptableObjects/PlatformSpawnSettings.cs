using UnityEngine;

[System.Serializable]
public class PlatformSpawnSettings
{
    public GameObject Prefab;
    public bool IsReliable = true;
    public float Weight = 1.0f;

    public PlatformSpawnSettings Clone()
    {
        return new PlatformSpawnSettings
        {
            Prefab = this.Prefab,
            IsReliable = this.IsReliable,
            Weight = this.Weight
        };
    }
}