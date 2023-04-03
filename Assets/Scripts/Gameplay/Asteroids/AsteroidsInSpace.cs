using System;
using System.Collections.Generic;
using Gameplay.Asteroids.Factories;
using Gameplay.Asteroids.Scriptables;
using Gameplay.Mechanics.Timer;
using Gameplay.Services;
using Gameplay.Space.Generator;
using UnityEngine;
using Utilities.Mathematics;
using Utilities.Unity;

namespace Gameplay.Asteroids
{
    public class AsteroidsInSpace : IDisposable
    {
        private readonly SpawnPointsFinder _spawnPointsFinder;
        private readonly RandomDirectedAsteroidFactory _randomDirectedAsteroidFactory;
        private readonly TargetedAsteroidFactory _targetedAsteroidFactory;
        private readonly TimerFactory _timerFactory;
        private readonly PlayerLocator _playerLocator;

        private readonly int _asteroidsOnStartCount;
        private readonly AsteroidSpawnConfig _spawnConfig;

        private readonly List<Asteroid> _asteroids = new();

        private readonly AsteroidConfig _fastAsteroidConfig;

        private Vector3 _playerPosition;
        private Timer _playerTargetedAsteroidDelay;

        private const int MaxSpawnTriesPerAsteroid = 5;
        private const int MaxTriesToCreateStartAsteroids = 100;


        public AsteroidsInSpace(
            int asteroidsOnStartCount,
            AsteroidSpawnConfig spawnConfig,
            SpawnPointsFinder spawnPointsFinder,
            RandomDirectedAsteroidFactory asteroidFactory,
            TargetedAsteroidFactory targetedAsteroidFactory,
            TimerFactory timerFactory,
            PlayerLocator playerLocator)
        {
            _asteroidsOnStartCount = asteroidsOnStartCount;
            _spawnConfig = spawnConfig;
            _spawnPointsFinder = spawnPointsFinder;
            _randomDirectedAsteroidFactory = asteroidFactory;
            _targetedAsteroidFactory = targetedAsteroidFactory;
            _timerFactory = timerFactory;
            _playerLocator = playerLocator;

            _fastAsteroidConfig = spawnConfig.FastAsteroid;
            _playerLocator.PlayerPosition += GetPlayerPosition;
        }

        public void Dispose()
        {
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
                if (!TrySpawnStartAsteroid(_spawnConfig)) tryCount++;
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

        private bool TrySpawnStartAsteroid(AsteroidSpawnConfig config)
        {
            var newAsteroidConfig = RandomPicker.PickOneElementByWeights(config.AsteroidConfigs);
            var asteroidCollider = newAsteroidConfig.Prefab.GetComponent<CircleCollider2D>();
            var asteroidRadius = asteroidCollider.radius;
            var asteroidSpawnRadius = RandomPicker.PickRandomBetweenTwoValues(0, newAsteroidConfig.MaxSpawnRadius);
            var spawnTries = 0;
            do
            {
                if (_spawnPointsFinder.TryGetSpaceObjectSpawnPoint(asteroidRadius, asteroidSpawnRadius, out var spawnPoint))
                {
                    var newAsteroid = _randomDirectedAsteroidFactory.Create(spawnPoint, newAsteroidConfig);
                    _asteroids.Add(newAsteroid);
                    return true;
                }
                spawnTries++;
            }
            while (spawnTries < MaxSpawnTriesPerAsteroid);
            return false;
        }

        private void SpawnPlayerTargetedAsteroid()
        {
            _playerTargetedAsteroidDelay.Start();
            var tryCount = 0;
            do
            {
                if (!TrySpawnFastAsteroid(_fastAsteroidConfig, _playerPosition)) tryCount++;
                else return;
            }
            while (tryCount < MaxSpawnTriesPerAsteroid);
        }

        private bool TrySpawnFastAsteroid(AsteroidConfig config, Vector2 baseSpawnPoint)
        {
            var asteroidCollider = config.Prefab.GetComponent<CircleCollider2D>();
            var asteroidRadius = asteroidCollider.radius;
            if (TryGetSpawnPointOnRadius(baseSpawnPoint, asteroidRadius, config.MaxSpawnRadius, out var spawnPoint))
            {
                var newAsteroid = _targetedAsteroidFactory.Create(spawnPoint, config, baseSpawnPoint);
                _asteroids.Add(newAsteroid);
                return true;
            }
            return false;
        }

        private bool TryGetSpawnPointOnRadius(Vector3 basePosition, float asteroidRadius, float spawnRadius, out Vector2 spawnPoint)
        {
            int spawnTries = 0;
            do
            {
                spawnPoint = UnityHelper.GetAPointOnRadius(basePosition, spawnRadius);
                spawnTries++;
            }
            while (UnityHelper.IsAnyObjectAtPosition(spawnPoint, asteroidRadius) && spawnTries <= MaxSpawnTriesPerAsteroid);

            if (spawnTries > MaxSpawnTriesPerAsteroid) return false;
            else return true;
        }
    }
}