using Abstracts;
using Asteroid;
using Gameplay.Mechanics.Timer;
using Gameplay.Player;
using Scriptables;
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
    public class GeneralAsteroidsController : BaseController
    {
        private readonly ResourcePath _asteroidsSpawnConfigPath = new(Constants.Configs.Asteroid.AsteroidsSpawnConfig);
        private readonly AsteroidFactory _asteroidFactory;
        private readonly AsteroidsSpawnConfig _config;
        private readonly Timer _timer;

        private readonly List<Vector3> _asteroidsSpawnPoints;
        const int _asteroidSpawnRadius = 20;
        private Dictionary<string, AsteroidController> _asteroidControllers = new();

        private readonly SingleAsteroidConfig _fastAsteroidConfig;
        private bool _appIsQuiting = false;

        const int max_empty_spawn_point_get_tries = 5;

        public GeneralAsteroidsController(PlayerController player, List<Vector3> asteroidsSpawnPoints)
        {
            GameObject asteroidsPool = new("AsteroidsPool");
            asteroidsPool.transform.position = new(9999, 9999);

            _config = ResourceLoader.LoadObject<AsteroidsSpawnConfig>(_asteroidsSpawnConfigPath);

            _fastAsteroidConfig = GetConfigByType(AsteroidType.FastAsteroid, _config.WeightConfigs);

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
                var asteroidToSpawn = RandomPicker.PickOneElementByWeights(_config.WeightConfigs, new());

                switch (asteroidToSpawn.ConfigType)
                {
                    case AsteroidConfigType.None:
                        throw new Exception("Config type is not defiend");

                    case AsteroidConfigType.SingleAsteroidConfig:
                        var config = asteroidToSpawn as SingleAsteroidConfig;
                        if (config.Equals(_fastAsteroidConfig)) break;
                        TrySpawnAsteroid(config);
                        break;

                    case AsteroidConfigType.AsteroidCloudConfig:
                        var cloudConfig = asteroidToSpawn as AsteroidCloudConfig;
                        TrySpawnAsteroidCloud(cloudConfig);
                        break;

                    default:
                        throw new Exception("No such config type found");
                }

            }

            _timer.Start();
        }

        private void SpawnNewFastAsteroid()
        {
            var newAsteroid = _asteroidFactory.CreateAsteroidOnDistanceFromPlayer(_fastAsteroidConfig);
            _asteroidControllers.Add(newAsteroid.Id, newAsteroid);
            _timer.Start();
        }

        private void RegisterAsteroidController(AsteroidController spawnedAsteroid)
        {
            _asteroidControllers.Add(spawnedAsteroid.Id, spawnedAsteroid);
            spawnedAsteroid.OnDestroy += DeleteAsteroidController;
        }

        private void RegisterAsteroidControllers(List<AsteroidController> asteroidCloudAsteroids)
        {
            for (int j = 0; j < asteroidCloudAsteroids.Count; j++)
            {
                _asteroidControllers.Add(asteroidCloudAsteroids[j].Id, asteroidCloudAsteroids[j]);
                asteroidCloudAsteroids[j].OnDestroy += DeleteAsteroidController;
            }
        }

        private void DeleteAsteroidController(AsteroidController asteroidController)
        {
            if (!_appIsQuiting && asteroidController.Config.CloudOnDestroy != null)
            {
                var asteroidControllers = SpawnSpecificAsteroidCloudOnDestroy(asteroidController);
                RegisterAsteroidControllers(asteroidControllers);
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
                tryCount++;
            }
            while (!UnityHelper.IsAnyObjectAtPosition(asteroidSpawnPoint, asteroidMaxSize) && tryCount <= max_empty_spawn_point_get_tries);

            if (tryCount > max_empty_spawn_point_get_tries) return false;
            else return true;
        }

        private void SetAppQuitTrigger() => _appIsQuiting = true;

        private SingleAsteroidConfig GetConfigByType(AsteroidType asteroidType, List<WeightConfig<AsteroidConfig>> configList)
        {
            Dictionary<AsteroidType, AsteroidConfig> asteroidTypeConfigPairs = new();

            for (int i = 0; i < configList.Count; i++)
            {
                var currentAsteroidConfig = _config.WeightConfigs[i].Config;

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

        private void TrySpawnAsteroid(SingleAsteroidConfig config)
        {
            if (GetEmptySpawnPoint(_asteroidsSpawnPoints, config.Size.AsteroidScale, out Vector3 spawnPoint))
            {
                var spawnedAsteroid = _asteroidFactory.CreateAsteroid(spawnPoint, config);
                RegisterAsteroidController(spawnedAsteroid);
            }
        }

        private void TrySpawnAsteroidCloud(AsteroidCloudConfig config)
        {
            if (GetEmptySpawnPoint(_asteroidsSpawnPoints, config.AsteroidCloudSize, out Vector3 spawnPoint))
            {
                var asteroidCloudAsteroids = _asteroidFactory.CreateAsteroidCloud(spawnPoint, config);
                RegisterAsteroidControllers(asteroidCloudAsteroids);
            }
        }

        private List<AsteroidController> SpawnSpecificAsteroidCloudOnDestroy(AsteroidController asteroidController)
        {
            if (asteroidController.LastCollision != null) 
                return _asteroidFactory.CreateAsteroidCloudAfterCollision(
                    asteroidController.View.transform.position,
                    asteroidController.Config.CloudOnCollisionDestroy,
                    asteroidController.LastCollision);

            else 
                return _asteroidFactory.CreateAsteroidCloudAfterAsteroidDestroyed(
                    asteroidController.View,
                    asteroidController.Config.CloudOnDestroy);
        }

        #endregion
    }
}
