using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Generation Settings Effect")]
public class GenerationSettingsEffectSO : EffectSO
{
    [SerializeField] private List<PlatformSpawnSettings> _platformSpawnSettingsList;

    public List<PlatformSpawnSettings> PlatformSpawnSettings => _platformSpawnSettingsList;

    public override void Apply(EffectContext context)
    {
        context.Settings.AddEffect(this);
    }

    public override void Remove(EffectContext context)
    {
        context.Settings.RemoveEffect(this);
    }
}