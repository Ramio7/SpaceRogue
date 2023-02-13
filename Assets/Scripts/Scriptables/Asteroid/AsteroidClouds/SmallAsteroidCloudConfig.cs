using Asteroid;
using System;
using UnityEngine;


namespace Scriptables.Asteroid
{
    [CreateAssetMenu(fileName = nameof(SmallAsteroidCloudConfig), menuName = "Configs/Asteroids/" + nameof(SmallAsteroidCloudConfig))]
    [Serializable]
    public sealed class SmallAsteroidCloudConfig : AsteroidCloudConfig
    {
        public SmallAsteroidCloudConfig()
        {
            CloudType = AsteroidCloudType.SmallAsteroidCloud;
            base.MinAsteroidsInCloud = this.MinAsteroidsInCloud;
            base.MaxAsteroidsInCloud = this.MaxAsteroidsInCloud;
        }

        [field: SerializeField, Range(5, 10)] public new int MinAsteroidsInCloud { get; private set; } = 5;
        [field: SerializeField, Min(10)] public new int MaxAsteroidsInCloud { get; private set; } = 10;
    }
}
