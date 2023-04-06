using Gameplay.Asteroids.Factories;
using Gameplay.Asteroids.Scriptables;
using Gameplay.Space.Generator;
using UnityEngine;
using Utilities.Mathematics;
using Utilities.Unity;
using Scriptables;
using System.Collections.Generic;
using System;

namespace Gameplay.Asteroids
{
    public class AsteroidSpawner
    {
        private readonly AsteroidFactory _asteroidFactory;
        private readonly SpawnPointsFinder _spawnPointsFinder;

        private const int MaxSpawnTriesPerAsteroid = 5;

        public event Action<Asteroid> AsteroidSpawned;

        public AsteroidSpawner(SpawnPointsFinder spawnPointsFinder, AsteroidFactory asteroidFactory)
        {
            _spawnPointsFinder = spawnPointsFinder;
            _asteroidFactory = asteroidFactory;
        }

        public bool SpawnRandomDirectedAsteroid(AsteroidSpawnConfig config)
        {
            var newAsteroidConfig = RandomPicker.PickOneElementByWeights(config.AsteroidSpawnConfigs);
            var asteroidCollider = newAsteroidConfig.Prefab.GetComponent<CircleCollider2D>();
            var asteroidRadius = asteroidCollider.radius;
            var asteroidSpawnRadius = RandomPicker.PickRandomBetweenTwoValues(0, newAsteroidConfig.MaxSpawnRadius);
            var spawnTries = 0;
            do
            {
                if (_spawnPointsFinder.TryGetSpaceObjectSpawnPoint(asteroidRadius, asteroidSpawnRadius, out var spawnPoint))
                {
                    var newAsteroid = _asteroidFactory.Create(spawnPoint, newAsteroidConfig);
                    newAsteroid.StartRandomDirectedMovement(newAsteroidConfig.StartingSpeed);
                    AsteroidSpawned.Invoke(newAsteroid);
                    return true;
                }
                spawnTries++;
            }
            while (spawnTries < MaxSpawnTriesPerAsteroid);
            return false;
        }

        public void SpawnTargetedAsteroid(List<WeightConfig<AsteroidConfig>> configs, Vector2 targetPosition)
        {
            var tryCount = 0;
            do
            {
                var fastAsteroidConfig = RandomPicker.PickOneElementByWeights(configs);
                if (!TrySpawnTargetedAsteroid(fastAsteroidConfig, targetPosition)) tryCount++;
                else return;
            }
            while (tryCount < MaxSpawnTriesPerAsteroid);
        }

        private bool TrySpawnTargetedAsteroid(AsteroidConfig config, Vector2 targetPosition)
        {
            var asteroidCollider = config.Prefab.GetComponent<CircleCollider2D>();
            var asteroidRadius = asteroidCollider.radius;
            if (TryGetSpawnPointOnRadius(targetPosition, asteroidRadius, config.MaxSpawnRadius, out var spawnPoint))
            {
                var newAsteroid = _asteroidFactory.Create(spawnPoint, config);
                newAsteroid.StartTargetedMovement(config.StartingSpeed, config.DeviationRadius, targetPosition);
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