using Abstracts;
using Asteroid;
using Gameplay.Mechanics.Timer;
using Gameplay.Player;
using Scriptables.Asteroid;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Mathematics;
using Utilities.ResourceManagement;
using Utilities.Unity;
using Random = UnityEngine.Random;


namespace Gameplay.Asteroid
{
    public class AsteroidsController : BaseController
    {
        private readonly ResourcePath _asteroidsSpawnConfigPath = new(Constants.Configs.Asteroid.AsteroidsSpawnConfig);
        private readonly AsteroidFactory _asteroidFactory;
        private readonly AsteroidsSpawnConfig _config;
        private readonly System.Random _random = new();
        private readonly Timer _timer;

        private readonly List<Vector3> _asteroidsSpawnPoints;
        const int _asteroidSpawnRadius = 20;
        private List<AsteroidController> _asteroidsControllers = new();

        private readonly SingleAsteroidConfig _fastAsteroidConfig;
        private bool _appIsQuiting = false;

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
            _timer.OnExpire += SpawnNewFastAsteroid;
            EntryPoint.SubscribeToApplicationQuit(SetAppQuitTrigger);
        }

        protected override void OnDispose()
        {
            _asteroidFactory.Dispose();
            DisposeAsteroidControllers();
            _timer.Dispose();
            EntryPoint.UnsubscribeToApplicationQuit(SetAppQuitTrigger);
        }

        private void DisposeAsteroidControllers()
        {
            for (int i = 0; i < _asteroidsControllers.Count; i++)
            {
                _asteroidsControllers[i].Dispose();
            }
            _asteroidsControllers.Clear();
        }

        #region MainMethods
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
                            throw new Exception("Config type is not defiend");

                        case AsteroidConfigType.SingleAsteroidConfig:
                            var config = currentAsteroidConfig as SingleAsteroidConfig;

                            if (config.Equals(_fastAsteroidConfig)) break;

                            TrySpawnAsteroid(config, _random);
                            break;

                        case AsteroidConfigType.AstreoidCloudConfig:
                            var cloudConfig = currentAsteroidConfig as AsteroidCloudConfig;

                            TrySpawnAsteroidCloud(cloudConfig, _random);
                            break;

                        default:
                            throw new Exception("No such config type found");
                    }
                }

            }

            _timer.Start();
        }

        private void SpawnNewFastAsteroid()
        {
            _asteroidsControllers.Add(_asteroidFactory.CreateAsteroidNearPlayer(_fastAsteroidConfig));
            _timer.Start();
        }

        private void DeleteAsteroidController(AsteroidController asteroidController)
        {
            if (!_appIsQuiting && asteroidController.Config.Cloud != null)
            {
                var view = asteroidController.View;
                var cloud = asteroidController.Config.Cloud;
                var asteroidControllers = SelectAsteroidsMoveBehaviour(asteroidController, view, cloud);
                RegisterAsteroidController(asteroidControllers);
            }

            _asteroidsControllers.Remove(asteroidController);
            asteroidController.Dispose();
        }
        #endregion

        #region SupportMethods
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

        private void SetAppQuitTrigger() => _appIsQuiting = true;

        private SingleAsteroidConfig GetConfigByType(AsteroidType asteroidType, List<AsteroidConfig> configList)
        {
            Dictionary<AsteroidType, AsteroidConfig> asteroidTypeConfigPairs = new();

            for (int i = 0; i < configList.Count; i++)
            {
                var currentAsteroidConfig = _config.AsteroidConfigs[i];

                switch (currentAsteroidConfig.ConfigType)
                {
                    case AsteroidConfigType.None:
                        throw new Exception("Config type is not defiend");
                    case AsteroidConfigType.SingleAsteroidConfig:
                        {
                            var singleAsteroid = currentAsteroidConfig as SingleAsteroidConfig;
                            if (asteroidTypeConfigPairs.ContainsKey(singleAsteroid.AsteroidType)) break;
                            asteroidTypeConfigPairs.Add(singleAsteroid.AsteroidType, singleAsteroid);
                            break;
                        }
                    case AsteroidConfigType.AstreoidCloudConfig:
                        break;
                    default:
                        throw new Exception("No such config type found");
                }
            }
            asteroidTypeConfigPairs.TryGetValue(asteroidType, out var configOutput);

            return (SingleAsteroidConfig)configOutput;
        }

        private void RegisterAsteroidController(AsteroidController spawnedAsteroid)
        {
            _asteroidsControllers.Add(spawnedAsteroid);
            spawnedAsteroid.Id = _asteroidsControllers.Count - 1;
            spawnedAsteroid.OnDestroy += DeleteAsteroidController;
        }

        private void RegisterAsteroidController(List<AsteroidController> asteroidCloudAsteroids)
        {
            for (int j = 0; j < asteroidCloudAsteroids.Count; j++)
            {
                _asteroidsControllers.Add(asteroidCloudAsteroids[j]);
                asteroidCloudAsteroids[j].Id = _asteroidsControllers.Count - 1;
                asteroidCloudAsteroids[j].OnDestroy += DeleteAsteroidController;
            }
        }

        private void TrySpawnAsteroid(SingleAsteroidConfig config, System.Random random)
        {
            if (RandomPicker.TakeChance(config.SpawnChance, random))
            {
                var spawnPoint = GetEmptySpawnPoint(_asteroidsSpawnPoints, config.Size.AsteroidScale, out Vector3 spawnCancel);
                if (spawnPoint == spawnCancel) return;

                var spawnedAsteroid = _asteroidFactory.CreateAsteroid(spawnPoint, config);
                RegisterAsteroidController(spawnedAsteroid);
            }
        }

        private void TrySpawnAsteroidCloud(AsteroidCloudConfig config, System.Random random)
        {
            if (config.Behavior is AsteroidCloudBehaviour.CreatorEscaping or AsteroidCloudBehaviour.CollisionEscaping) return;

            if (RandomPicker.TakeChance(config.SpawnChance, random))
            {
                var spawnPoint = GetEmptySpawnPoint(_asteroidsSpawnPoints, config.AsteroidCloudSize, out Vector3 spawnCancel);
                if (spawnPoint == spawnCancel) return;

                var asteroidCloudAsteroids = _asteroidFactory.CreateAsteroidCloud(spawnPoint, config);
                RegisterAsteroidController(asteroidCloudAsteroids);
            }
        }

        private List<AsteroidController> SelectAsteroidsMoveBehaviour(AsteroidController asteroidController, AsteroidView view, AsteroidCloudConfig cloud)
        {
            return asteroidController.Config.Cloud.Behavior switch
            {
                AsteroidCloudBehaviour.None => throw new Exception("Cloud behaviour not set"),
                AsteroidCloudBehaviour.Static => _asteroidFactory.CreateAsteroidCloud(view, cloud),
                AsteroidCloudBehaviour.CreatorEscaping => _asteroidFactory.CreateAsteroidCloud(view, cloud),
                _ => throw new NotImplementedException(),
            };
        }
        #endregion
    }
}
