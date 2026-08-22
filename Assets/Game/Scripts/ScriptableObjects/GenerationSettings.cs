using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenerationSettings", menuName = "Game/Generation Settings")]
public class GenerationSettings : ScriptableObject
{
    [SerializeField] private List<GenerationZone> _zones;

    public List<GenerationZone> Zones => _zones;
}