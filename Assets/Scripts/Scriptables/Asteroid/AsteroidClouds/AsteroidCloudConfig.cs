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
        [NonSerialized] public AsteroidCloudType CloudType;
        [NonSerialized] public int MinAsteroidsInCloud;
        [NonSerialized] public int MaxAsteroidsInCloud;
        [field: SerializeField] public bool SpawnAsteroidCloudOnStart { get; private set; }
        [field: SerializeField] public AsteroidCloudBehaviour Behavior { get; private set; }
        [field: SerializeField] public Vector3 AsteroidCloudSize { get; private set; }
        [field: SerializeField] public List<SingleAsteroidConfig> CloudAsteroidsConfigs { get; private set; }
    }
}
