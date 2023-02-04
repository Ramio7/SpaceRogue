using Abstracts;
using Gameplay.Player;
using Scriptables.Asteroid;
using UnityEngine;
using Utilities.Mathematics;
using Utilities.Unity;

namespace Gameplay.Asteroid
{
    public class CloudAsteroidsSpawnController : BaseController
    {
        private readonly AsteroidFactory _asteroidFactory;
        private readonly AsteroidExplosionController _asteroidExplosionController;
        private readonly AsteroidsSpawnConfig _asteroidSpawnConfig;
        private readonly PlayerView _playerView;

        public CloudAsteroidsSpawnController(PlayerView playerView, AsteroidsSpawnConfig asteroidSpawnConfig, AsteroidExplosionController asteroidExplosionController)
        {
            var cloudSpawnConfig = asteroidSpawnConfig;
            _asteroidExplosionController = asteroidExplosionController;
            _asteroidSpawnConfig = asteroidSpawnConfig;

            _playerView = playerView;

            //foreach (var AsteroidGroupSpawn in cloudSpawnConfig.AsteroidCloudsSpawnConfig)
            //{
            //    var currentcloudSpawnConfig = cloudSpawnConfig.AsteroidClouds[Random.Range(0, cloudSpawnConfig.AsteroidClouds.Count)];
            //    _asteroidFactory = new AsteroidFactory(currentcloudSpawnConfig);

            //    var unitSize = currentcloudSpawnConfig.Prefab.transform.localScale;

            //    int spawnCircleRadius = Random.Range(AsteroidGroupSpawn.MinimumRadius, AsteroidGroupSpawn.MaximumRadius);

            //    for (int i = 0; i < AsteroidGroupSpawn.GroupCount; i++)
            //    {
            //        var unitSpawnPoint = GetEmptySpawnPoint(AsteroidGroupSpawn.GroupSpawnPoint, unitSize, spawnCircleRadius);
            //        var asteroidController = _asteroidFactory.CreateAsteroid(unitSpawnPoint, playerView, _asteroidExplosionController);
            //        AddController(asteroidController);
            //    }
            //}
        }

        public void CloudAsteroidsSpawn(Vector3 spawnPosition)
        {
            var cloudSpawnConfig = _asteroidSpawnConfig;

            //var currentcloudSpawnConfig = cloudSpawnConfig.AsteroidClouds[Random.Range(0, cloudSpawnConfig.AsteroidClouds.Count)];
            //var unitSize = currentcloudSpawnConfig.Prefab.transform.localScale;
            //int spawnCircleRadius = Random.Range(cloudSpawnConfig.SmallCloudConfig.MinimumRadius, cloudSpawnConfig.SmallCloudConfig.MaximumRadius);

            //for (int i = 0; i < Random.Range(cloudSpawnConfig.SmallCloudConfig.MinimumCount, cloudSpawnConfig.SmallCloudConfig.MaximumCount); i++)
            //{
            //    var unitSpawnPoint = GetEmptySpawnPoint(spawnPosition, unitSize, spawnCircleRadius);
            //    var asteroidController = _asteroidFactory.CreateAsteroid(unitSpawnPoint, _playerView, _asteroidExplosionController);
            //    AddController(asteroidController);
            //}
        }

        private static Vector3 GetEmptySpawnPoint(Vector3 spawnPoint, Vector3 unitSize, int spawnCircleRadius)
        {
            var unitSpawnPoint = spawnPoint + (Vector3)(Random.insideUnitCircle * spawnCircleRadius);
            float unitMaxSize = unitSize.MaxVector3CoordinateOnPlane();

            while (UnityHelper.IsAnyObjectAtPosition(unitSpawnPoint, unitMaxSize))
            {
                unitSpawnPoint = spawnPoint + (Vector3)(Random.insideUnitCircle * spawnCircleRadius);
            }

            return unitSpawnPoint;
        }

    }
}