using Gameplay.Asteroids;
using Gameplay.Asteroids.Scriptables;
using Gameplay.Mechanics.Timer;
using Scriptables;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Services
{
    public class PlayerTargetedAsteroidSpawner : IDisposable
    {
        private readonly AsteroidSpawner _spawner;
        private readonly PlayerLocator _playerLocator;
        private readonly TimerFactory _timerFactory;
        private readonly Timer _playerTargetedAsteroidDelay;
        private readonly List<WeightConfig<AsteroidConfig>> _fastAsteroidConfigs;

        private Vector3 _playerPosition;

        public PlayerTargetedAsteroidSpawner(
            AsteroidSpawner spawner, 
            PlayerLocator playerLocator, 
            TimerFactory timerFactory,
            AsteroidSpawnConfig spawnConfig,
            float spawnDelay)
        {
            _spawner = spawner;
            _playerLocator = playerLocator;
            _timerFactory = timerFactory;
            _fastAsteroidConfigs = spawnConfig.FastAsteroidConfigs;
            _playerTargetedAsteroidDelay = _timerFactory.Create(spawnDelay);

            _playerLocator.PlayerPosition += GetPlayerPosition;
        }

        public void Dispose()
        {
            _playerLocator.PlayerPosition -= GetPlayerPosition;
            _playerTargetedAsteroidDelay.OnExpire -= SpawnPlayerTargetedAsteroid;

            _playerLocator.Dispose();
            _playerTargetedAsteroidDelay.Dispose();
            _fastAsteroidConfigs.Clear();
        }

        public void StartPlayerTargetedAsteroidsSpawn()
        {
            _playerTargetedAsteroidDelay.Start();
            _playerTargetedAsteroidDelay.OnExpire += SpawnPlayerTargetedAsteroid;
        }

        private void GetPlayerPosition(Vector3 playerPosition) => _playerPosition = playerPosition;

        private void SpawnPlayerTargetedAsteroid()
        {
            _spawner.SpawnTargetedAsteroid(_fastAsteroidConfigs, _playerPosition);
            _playerTargetedAsteroidDelay.Start();
        }

    }
}