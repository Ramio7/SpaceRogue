using Asteroid;
using System;
using UnityEngine;


namespace Scriptables.Asteroid
{
    [CreateAssetMenu(fileName = nameof(BigAsteroidCloudConfig), menuName = "Configs/Asteroids/" + nameof(BigAsteroidCloudConfig))]
    [Serializable]
    public sealed class BigAsteroidCloudConfig : AsteroidCloudConfig
    {
        public BigAsteroidCloudConfig()
        {
            CloudType = AsteroidCloudType.BigAsteroidCloud;
            base.MinAsteroidsInCloud = MinAsteroidsInCloud;
            base.MaxAsteroidsInCloud = MaxAsteroidsInCloud;
        }
        [field: SerializeField, Range(12, 25)] public new int MinAsteroidsInCloud { get; private set; } = 12;
        [field: SerializeField, Min(25)] public new int MaxAsteroidsInCloud { get; private set; } = 25;
    }
}