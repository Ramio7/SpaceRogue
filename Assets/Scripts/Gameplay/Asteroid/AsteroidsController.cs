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
        private Dictionary<string, AsteroidController> _asteroidControllers = new();

        private readonly SingleAsteroidConfig _fastAsteroidConfig;
        private bool _appIsQuiting = false;

        const int max_empty_spawn_point_get_tries = 5;

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
            foreach (var asteroidController in _asteroidControllers) asteroidController.Value.Dispose();
            _asteroidControllers.Clear();
        }



        #region MainMethods

        private void SpawnStartAsteroids()
        {
            while (_config.MaxAsteroidsInSpace > _asteroidControllers.Count)
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

                        case AsteroidConfigType.AsteroidCloudConfig:
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
            var newAsteroid = _asteroidFactory.CreateAsteroidNearPlayer(_fastAsteroidConfig);
            _asteroidControllers.Add(newAsteroid.Id, newAsteroid);
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

            _asteroidControllers.Remove(asteroidController.Id);
            asteroidController.Dispose();
        }

        #endregion


        #region SupportMethods

        private bool GetEmptySpawnPoint(List<Vector3> spawnPoints, Vector3 asteroidSize, out Vector3 asteroidSpawnPoint)
        {
            int tryCount = 0;

            float asteroidMaxSize = asteroidSize.MaxVector3CoordinateOnPlane();

            do
            {
                var randomSpawnPoint = Random.Range(0, spawnPoints.Count);

                asteroidSpawnPoint = spawnPoints[randomSpawnPoint] + (Vector3)Random.insideUnitCircle * _asteroidSpawnRadius;
            }
            while (!UnityHelper.IsAnyObjectAtPosition(asteroidSpawnPoint, asteroidMaxSize) && tryCount <= max_empty_spawn_point_get_tries);

            if (tryCount > max_empty_spawn_point_get_tries) return false;
            else return true;
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
                    case AsteroidConfigType.AsteroidCloudConfig:
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
            _asteroidControllers.Add(spawnedAsteroid.Id, spawnedAsteroid);
            spawnedAsteroid.OnDestroy += DeleteAsteroidController;
        }

        private void RegisterAsteroidController(List<AsteroidController> asteroidCloudAsteroids)
        {
            for (int j = 0; j < asteroidCloudAsteroids.Count; j++)
            {
                _asteroidControllers.Add(asteroidCloudAsteroids[j].Id, asteroidCloudAsteroids[j]);
                asteroidCloudAsteroids[j].OnDestroy += DeleteAsteroidController;
            }
        }

        private void TrySpawnAsteroid(SingleAsteroidConfig config, System.Random random)
        {
            if (RandomPicker.TakeChance(config.SpawnChance, random))
            {
                if (GetEmptySpawnPoint(_asteroidsSpawnPoints, config.Size.AsteroidScale, out Vector3 spawnPoint)) 
                {
                    var spawnedAsteroid = _asteroidFactory.CreateAsteroid(spawnPoint, config);
                    RegisterAsteroidController(spawnedAsteroid);
                }
            }
        }

        private void TrySpawnAsteroidCloud(AsteroidCloudConfig config, System.Random random)
        {
            if (config.Behavior is AsteroidCloudBehaviour.CreatorEscaping or AsteroidCloudBehaviour.CollisionEscaping) return;

            if (RandomPicker.TakeChance(config.SpawnChance, random))
            {
                if (GetEmptySpawnPoint(_asteroidsSpawnPoints, config.AsteroidCloudSize, out Vector3 spawnPoint))
                {
                    var asteroidCloudAsteroids = _asteroidFactory.CreateAsteroidCloud(spawnPoint, config);
                    RegisterAsteroidController(asteroidCloudAsteroids);
                }
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
