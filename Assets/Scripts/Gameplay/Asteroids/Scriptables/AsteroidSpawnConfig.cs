using System.Collections.Generic;
using Scriptables;
using UnityEngine;

namespace Gameplay.Asteroids.Scriptables
{
    [CreateAssetMenu(fileName = nameof(AsteroidSpawnConfig), menuName = "Configs/Asteroids/" + nameof(AsteroidSpawnConfig))]
    public class AsteroidSpawnConfig : ScriptableObject
    {
        [field: SerializeField] public List<WeightConfig<AsteroidConfig>> AsteroidSpawnConfigs { get; private set; }
        [field: SerializeField] public List<WeightConfig<AsteroidConfig>> FastAsteroidConfigs { get; private set; }
    }
}