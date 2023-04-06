using System;
using System.Collections.Generic;
using Gameplay.Asteroids.Scriptables;
using Gameplay.Mechanics.Timer;
using Gameplay.Services;
using Scriptables;
using UnityEngine;

namespace Gameplay.Asteroids
{
    public class AsteroidsInSpace : IDisposable
    {
        private readonly TimerFactory _timerFactory;
        private readonly PlayerLocator _playerLocator;
        private readonly AsteroidSpawner _spawner;

        private readonly int _asteroidsOnStartCount;
        private readonly AsteroidSpawnConfig _spawnConfig;

        private readonly List<Asteroid> _asteroids = new();
        private readonly List<WeightConfig<AsteroidConfig>> _fastAsteroidConfigs;

        private Vector3 _playerPosition;
        private Timer _playerTargetedAsteroidDelay;

        private const int MaxTriesToCreateStartAsteroids = 100;


        public AsteroidsInSpace(
            int asteroidsOnStartCount,
            AsteroidSpawnConfig spawnConfig,
            TimerFactory timerFactory,
            PlayerLocator playerLocator,
            AsteroidSpawner spawner)
        {
            _asteroidsOnStartCount = asteroidsOnStartCount;
            _spawnConfig = spawnConfig;
            _timerFactory = timerFactory;
            _playerLocator = playerLocator;
            _spawner = spawner;

            _fastAsteroidConfigs = _spawnConfig.FastAsteroidConfigs;
            _playerLocator.PlayerPosition += GetPlayerPosition;
            _spawner.AsteroidSpawned += GetNewAsteroid;
        }

        public void Dispose()
        {
            _playerLocator.PlayerPosition -= GetPlayerPosition;
            _spawner.AsteroidSpawned -= GetNewAsteroid;

            foreach (var asteroid in _asteroids)
            {
                asteroid.Dispose();
            }
            _asteroids.Clear();
        }

        public void SpawnStartAsteroids()
        {
            var tryCount = 0;
            do
            {
                if (!_spawner.SpawnRandomDirectedAsteroid(_spawnConfig)) tryCount++;
            }
            while (_asteroids.Count < _asteroidsOnStartCount || tryCount < MaxTriesToCreateStartAsteroids);
        }

        public void StartPlayerTargetedAsteroidsSpawn(float spawnDelay)
        {
            _playerTargetedAsteroidDelay = _timerFactory.Create(spawnDelay);
            _playerTargetedAsteroidDelay.Start();
            _playerTargetedAsteroidDelay.OnExpire += SpawnPlayerTargetedAsteroid;
        }

        private void GetPlayerPosition(Vector3 playerPosition) => _playerPosition = playerPosition;

        private void GetNewAsteroid(Asteroid newAsteroid) => _asteroids.Add(newAsteroid);

        private void SpawnPlayerTargetedAsteroid()
        {
            _spawner.SpawnTargetedAsteroid(_fastAsteroidConfigs, _playerPosition);
            _playerTargetedAsteroidDelay.Start();
        }
    }
}