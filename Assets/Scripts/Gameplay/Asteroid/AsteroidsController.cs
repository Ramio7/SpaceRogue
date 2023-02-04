using Abstracts;
using Gameplay.Mechanics.Timer;
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
        private readonly Dictionary<AsteroidConfig, AsteroidFactory> _asteroidFactories = new();

        private readonly AsteroidsSpawnConfig _config;

        private int _asteroidsInSpace;
        private const int _maxSpawnTries = 5;
        private List<Vector3> _asteroidsSpawnPoints;
        private GameObject _asteroidsPool = new("AsteroidsPool");

        protected System.Random _random = new();
        protected Timer _timer;

        public AsteroidsController(List<Vector3> asteroidsSpawnPoints)
        {
            _asteroidsPool.transform.position = new(9999, 9999);
            _config = ResourceLoader.LoadObject<AsteroidsSpawnConfig>(_asteroidsSpawnConfigPath);
            _asteroidsSpawnPoints = asteroidsSpawnPoints;
            _asteroidsInSpace = _config.MaxAsteroidsInSpace;

            for (int i = 0; i < _config.AsteroidConfigs.Count; i++)
                _asteroidFactories.Add(_config.AsteroidConfigs[i], new AsteroidFactory(_config.AsteroidConfigs[i], _asteroidsPool));

            _timer = new(_config.AsteroidRespawnTime);
            _timer.Start();

            SpawnStartAsteroids();
            //EntryPoint.SubscribeToFixedUpdate(CheckNewAsteroidSpawn);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            _timer.Dispose();
        }

        private void SpawnStartAsteroids()
        {
            if (_config.MaxAsteroidsInSpace >= _asteroidsInSpace)
            {
                for (int i = 0; i < _config.AsteroidConfigs.Count; ++i)
                {
                    if (RandomPicker.TakeChance(_config.AsteroidConfigs[i].SpawnChanceWeight, _random))
                    {
                        for (int j = 0; j < _asteroidsSpawnPoints.Count; ++j)
                        {
                            var spawnPoint = GetEmptySpawnPoint(_asteroidsSpawnPoints[j], _config.AsteroidConfigs[i].AsteroidSize);
                            _asteroidFactories[_config.AsteroidConfigs[i]].CreateAsteroid(spawnPoint, _asteroidsPool);
                            _asteroidsInSpace++;
                        }
                    }

                    _timer.Start();
                }
            }
        }

        //private void CheckNewAsteroidSpawn()
        //{
        //    if (_timer.IsExpired)
        //    {
        //        for (int i = 0; i < _config.AsteroidConfigs.Count; ++i) 
        //        {
        //            if (RandomPicker.TakeChance(_config.AsteroidConfigs[i].SpawnChanceWeight, _random))
        //            {
        //                var spawnPoint = GetEmptySpawnPoint(_config.AsteroidSpawnPoints, _config.AsteroidConfigs[i].AsteroidSize);
                        
        //                _asteroidsInSpace++;
        //            }

        //            _timer.Start();
        //        }
        //    }
        //}

        private static Vector3 GetEmptySpawnPoint(Vector3 spawnPoint, Vector3 asteroidSize)
        {
            int tryCount = 0;
            var asteroidSpawnPoint = spawnPoint + (Vector3)(Random.insideUnitCircle);
            float unitMaxSize = asteroidSize.MaxVector3CoordinateOnPlane();

            while (UnityHelper.IsAnyObjectAtPosition(asteroidSpawnPoint, unitMaxSize) && tryCount <= _maxSpawnTries)
            {
                asteroidSpawnPoint = spawnPoint + (Vector3)Random.insideUnitCircle;
                tryCount++;
            }

            return asteroidSpawnPoint;
        }
    }
}
