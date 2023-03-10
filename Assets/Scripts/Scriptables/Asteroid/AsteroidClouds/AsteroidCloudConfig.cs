using Asteroid;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Scriptables.Asteroid
{
    [CreateAssetMenu(fileName = nameof(AsteroidCloudConfig), menuName = "Configs/Asteroids/" + nameof(AsteroidCloudConfig))]
    [Serializable]
    public class AsteroidCloudConfig: AsteroidConfig
    {
        public AsteroidCloudConfig() => ConfigType = AsteroidConfigType.AsteroidCloudConfig;
        [field: SerializeField] public AsteroidCloudType CloudType { get; private set; }
        [field: SerializeField] public int MinAsteroidsInCloud { get; private set; }
        [field: SerializeField] public int MaxAsteroidsInCloud { get; private set; }
        [field: SerializeField] public bool SpawnAsteroidCloudOnStart { get; private set; }
        [field: SerializeField] public AsteroidCloudBehaviour Behavior { get; set; }
        [field: SerializeField] public Vector3 AsteroidCloudSize { get; private set; }
        [field: SerializeField] public List<WeightConfig<SingleAsteroidConfig>> CloudAsteroidsConfigs { get; private set; }
    }
}
