using System.Collections.Generic;
using UnityEngine;

namespace Scriptables.Asteroid
{
    [CreateAssetMenu(fileName = nameof(AsteroidsSpawnConfig), menuName = "Configs/Asteroid/" + nameof(AsteroidsSpawnConfig))]
    public class AsteroidsSpawnConfig : ScriptableObject
    {
        [field: SerializeField] public int MaxAsteroidsInSpace;
        [field: SerializeField] public float FastAsteroidSpawnDelay;
        [field: SerializeField] public List<AsteroidConfig> AsteroidConfigs;
    }
}