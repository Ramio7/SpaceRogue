using Asteroid;
using System;
using UnityEngine;

namespace Scriptables.Asteroid
{
    [Serializable]
    public class AsteroidCloudSpawn
    {
        [field: SerializeField] public Vector3 GroupSpawnPoint { get; private set; }
        [field: SerializeField, Min(1)] public int GroupCount { get; private set; }
        [field: SerializeField] public AsteroidCloudType AsteroidCloudType { get; private set; }
        [field: SerializeField, Min(1)] public int MinimumRadius { get; private set; }
        [field: SerializeField, Min(1)] public int MaximumRadius { get; private set; }
    }
}