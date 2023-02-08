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

        private int _asteroidsInSpace;

        private List<Vector3> _asteroidsSpawnPoints;

        private AsteroidConfig _fastAsteroidConfig;



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
            _timer.Dispose();
            EntryPoint.UnsubscribeFromFixedUpdate(SpawnNewFastAsteroid);
        }

        private void SpawnStartAsteroids()
        {
            while (_config.MaxAsteroidsInSpace >= _asteroidsInSpace)
            {
                for (int i = 0; i < _config.AsteroidConfigs.Count; i++)
                {
                    AsteroidConfig currentAsteroidConfig = _config.AsteroidConfigs[i];

                    if (currentAsteroidConfig.Equals(_fastAsteroidConfig)) break;

                    if (RandomPicker.TakeChance(currentAsteroidConfig.SpawnChance, _random))
                    {
                        var spawnPoint = GetEmptySpawnPoint(_asteroidsSpawnPoints, currentAsteroidConfig.Size.AsteroidScale);
                        _asteroidFactory.CreateAsteroid(spawnPoint, currentAsteroidConfig);
                        _asteroidsInSpace++;
                    }
                }
            }

            _timer.Start();
        }

        private void SpawnNewFastAsteroid()
        {
            if (_timer.IsExpired)
            {
                _asteroidFactory.CreateAsteroidNearPlayer(_fastAsteroidConfig);
                _timer.Start();
            }
        }

        private Vector3 GetEmptySpawnPoint(List<Vector3> spawnPoints, Vector3 asteroidSize)
        {
            Vector3 asteroidSpawnPoint = new();
            var spawnPointClaimed = false;
            float asteroidMaxSize = asteroidSize.MaxVector3CoordinateOnPlane();

            while (!spawnPointClaimed)
            {
                for (int i = 0; i < spawnPoints.Count; i++)
                {
                    if (!UnityHelper.IsAnyObjectAtPosition(spawnPoints[i], asteroidMaxSize))
                    {
                        spawnPointClaimed = true;
                        asteroidSpawnPoint = spawnPoints[i];
                        break;
                    }
                }
            }

            return asteroidSpawnPoint;
        }

        private AsteroidConfig GetConfigByType(AsteroidType asteroidType, List<AsteroidConfig> configList)
        {
            Dictionary<AsteroidType, AsteroidConfig> asteroidTypeConfigPairs = new();

            for (int i = 0; i < configList.Count; i++)
            {
                asteroidTypeConfigPairs.Add(configList[i].AsteroidType, configList[i]);
            }

            asteroidTypeConfigPairs.TryGetValue(asteroidType, out var config);

            return config;
        }
    }
}
