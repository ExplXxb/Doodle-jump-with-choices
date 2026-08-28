using UnityEngine;

[System.Serializable]
public class EnemySpawnSettings
{
    public GameObject Prefab;
    public float Weight = 1.0f;

    public EnemySpawnSettings Clone()
    {
        return new EnemySpawnSettings
        {
            Prefab = this.Prefab,
            Weight = this.Weight
        };
    }
}