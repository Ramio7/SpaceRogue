using Abstracts;
using Asteroid;
using Gameplay.Mechanics.Timer;
using Gameplay.Player;
using Scriptables.Asteroid;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Mathematics;
using Utilities.ResourceManagement;
using Utilities.Unity;


namespace Gameplay.Asteroid
{
    public class AsteroidsController : BaseController
    {
        private readonly ResourcePath _asteroidsSpawnConfigPath = new(Constants.Configs.Asteroid.AsteroidsSpawnConfig);
        private readonly AsteroidFactory _asteroidFactory;
        private readonly AsteroidsSpawnConfig _config;
        private readonly System.Random _random = new();
        private readonly Timer _timer;

        private List<Vector3> _asteroidsSpawnPoints;
        const int _asteroidSpawnRadius = 20;
        private List<AsteroidController> _asteroidsControllers = new();

        private SingleAsteroidConfig _fastAsteroidConfig;



        public AsteroidsController(PlayerController player, List<Vector3> asteroidsSpawnPoints)
        {
            GameObject asteroidsPool = new("AsteroidsPool");
            asteroidsPool.transform.position = new(9999, 9999);

            _config = ResourceLoader.LoadObject<AsteroidsSpawnConfig>(_asteroidsSpawnConfigPath);

            _fastAsteroidConfig = GetConfigByType(AsteroidType.FastAsteroid, _config.AsteroidConfigs);

            _asteroidFactory = new(asteroidsPool, player.View);

            _asteroidsSpawnPoints = asteroidsSpawnPoints;

            _timer = new(_config.FastAsteroidSpawnDelay);

            SpawnStartAsteroids();
            EntryPoint.SubscribeToFixedUpdate(SpawnNewFastAsteroid);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            _asteroidFactory.Dispose();
            for (int i = 0; i < _asteroidsControllers.Count; i++)
            {
                _asteroidsControllers[i].Dispose();
            }
            _asteroidsControllers.Clear();
            _timer.Dispose();
            EntryPoint.UnsubscribeFromFixedUpdate(SpawnNewFastAsteroid);
        }

        private void SpawnStartAsteroids()
        {
            while (_config.MaxAsteroidsInSpace > _asteroidsControllers.Count)
            {
                for (int i = 0; i < _config.AsteroidConfigs.Count; i++)
                {
                    var currentAsteroidConfig = _config.AsteroidConfigs[i];

                    switch (currentAsteroidConfig.ConfigType)
                    {
                        case AsteroidConfigType.None:
                            throw new System.Exception("Config type is not defiend");

                        case AsteroidConfigType.SingleAsteroidConfig:
                            var config = currentAsteroidConfig as SingleAsteroidConfig;

                            if (config.Equals(_fastAsteroidConfig)) break;

                            if (RandomPicker.TakeChance(config.SpawnChance, _random))
                            {
                                var spawnPoint = GetEmptySpawnPoint(_asteroidsSpawnPoints, config.Size.AsteroidScale, out Vector3 spawnCancel);
                                if (spawnPoint == spawnCancel) break;

                                _asteroidsControllers.Add(_asteroidFactory.CreateAsteroid(spawnPoint, config));
                            }
                            break;

                        case AsteroidConfigType.AstreoidCloudConfig:
                            var cloudConfig = currentAsteroidConfig as AsteroidCloudConfig;

                            if (RandomPicker.TakeChance(cloudConfig.SpawnChance, _random))
                            {
                                var spawnPoint = GetEmptySpawnPoint(_asteroidsSpawnPoints, cloudConfig.AsteroidCloudSize, out Vector3 spawnCancel);
                                if (spawnPoint == spawnCancel) break;

                                var asteroidCloudAsteroids = _asteroidFactory.CreateAsteroidCloud(spawnPoint, cloudConfig);

                                for (int j = 0; j < asteroidCloudAsteroids.Count; j++)
                                {
                                    _asteroidsControllers.Add(asteroidCloudAsteroids[j]);
                                }
                            }
                            break;

                        default:
                            throw new System.Exception("No such config type found");
                    }
                }
                
            }

            _timer.Start();
        }

        private void SpawnNewFastAsteroid()
        {
            if (_timer.IsExpired)
            {
                _asteroidsControllers.Add(_asteroidFactory.CreateAsteroidNearPlayer(_fastAsteroidConfig));
                _timer.Start();
            }
        }

        private Vector3 GetEmptySpawnPoint(List<Vector3> spawnPoints, Vector3 asteroidSize, out Vector3 cancellationToken)
        {
            Vector3 asteroidSpawnPoint = new();
            var spawnPointClaimed = false;

            int tryCount = 0;
            int maxTries = 5;

            float asteroidMaxSize = asteroidSize.MaxVector3CoordinateOnPlane();

            while (!spawnPointClaimed && tryCount < maxTries)
            {
                var randomSpawnPoint = Random.Range(0, spawnPoints.Count);

                var spawnPoint = spawnPoints[randomSpawnPoint] + (Vector3)Random.insideUnitCircle * _asteroidSpawnRadius;
                spawnPointClaimed = !UnityHelper.IsAnyObjectAtPosition(spawnPoint, asteroidMaxSize);

                if (spawnPointClaimed)
                {
                    asteroidSpawnPoint = spawnPoint;
                    break;
                }

                tryCount++;
            }

            if (tryCount >= maxTries)
            {
                cancellationToken = Vector3.one;
                return cancellationToken;
            }

            cancellationToken = Vector3.zero;
            return asteroidSpawnPoint;
        }

        private SingleAsteroidConfig GetConfigByType(AsteroidType asteroidType, List<AsteroidConfig> configList)
        {
            Dictionary<AsteroidType, AsteroidConfig> asteroidTypeConfigPairs = new();

            for (int i = 0; i < configList.Count; i++)
            {
                var currentAsteroidConfig = _config.AsteroidConfigs[i];

                switch (currentAsteroidConfig.ConfigType)
                {
                    case AsteroidConfigType.None:
                        throw new System.Exception("Config type is not defiend");

                    case AsteroidConfigType.SingleAsteroidConfig:
                        var singleAsteroid = currentAsteroidConfig as SingleAsteroidConfig;

                        if (asteroidTypeConfigPairs.ContainsKey(singleAsteroid.AsteroidType)) break;

                        asteroidTypeConfigPairs.Add(singleAsteroid.AsteroidType, singleAsteroid);
                        break;

                    case AsteroidConfigType.AstreoidCloudConfig:
                        break;

                    default:
                        throw new System.Exception("No such config type found");
                }
            }
            asteroidTypeConfigPairs.TryGetValue(asteroidType, out var configOutput);

            return (SingleAsteroidConfig)configOutput;
        }
    }
}
