using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Generation Settings Effect")]
public class GenerationSettingsEffectSO : EffectSO
{
    [SerializeField] private List<PlatformSpawnSettings> _platformSpawnSettingsList;

    public List<PlatformSpawnSettings> PlatformSpawnSettings => _platformSpawnSettingsList;

    public override void Apply()
    {
        GameSettings.Instance.AddEffect(this);
    }

    public override void Remove()
    {
        GameSettings.Instance.RemoveEffect(this);
    }
}