using Asteroid;
using System;
using UnityEngine;

namespace Scriptables.Asteroid
{
    [CreateAssetMenu(fileName = nameof(MediumAsteroidCloudConfig), menuName = "Configs/Asteroids/" + nameof(MediumAsteroidCloudConfig))]
    [Serializable]
    public class MediumAsteroidCloudConfig : AsteroidCloudConfig
    {
        public MediumAsteroidCloudConfig()
        {
            CloudType = AsteroidCloudType.SmallAsteroidCloud;
            base.MinAsteroidsInCloud = MinAsteroidsInCloud;
            base.MaxAsteroidsInCloud = MaxAsteroidsInCloud;
        }

        [field: SerializeField, Range(8, 15)] public new int MinAsteroidsInCloud { get; private set; } = 5;
        [field: SerializeField, Min(15)] public new int MaxAsteroidsInCloud { get; private set; } = 10;
    }
}