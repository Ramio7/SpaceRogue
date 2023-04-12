using System;
using System.Collections.Generic;
using Gameplay.Asteroids.Scriptables;

namespace Gameplay.Asteroids
{
    public class StartingAsteroids : IDisposable
    {
        private readonly int _asteroidsOnStartCount;
        private readonly AsteroidSpawner _spawner;
        private readonly AsteroidSpawnConfig _spawnConfig;

        private readonly Dictionary<string, Asteroid> _startingAsteroids = new();

        private const int MaxTriesToCreateStartAsteroids = 100;

        public StartingAsteroids(
            int asteroidsOnStartCount,
            AsteroidSpawnConfig spawnConfig,
            AsteroidSpawner spawner)
        {
            _asteroidsOnStartCount = asteroidsOnStartCount;
            _spawnConfig = spawnConfig;
            _spawner = spawner;

            _spawner.AsteroidSpawned += GetNewAsteroid;
        }

        public void Dispose()
        {
            _spawner.AsteroidSpawned -= GetNewAsteroid;

            foreach (var asteroid in _startingAsteroids.Values)
            {
                asteroid.Dispose();
            }
            _startingAsteroids.Clear();
        }

        public void SpawnStartAsteroids()
        {
            var tryCount = 0;
            do
            {
                if (!_spawner.SpawnRandomDirectedAsteroid(_spawnConfig)) tryCount++;
            }
            while (_startingAsteroids.Count < _asteroidsOnStartCount || tryCount < MaxTriesToCreateStartAsteroids);

            _spawner.AsteroidSpawned -= GetNewAsteroid;
        }

        private void GetNewAsteroid(Asteroid newAsteroid)
        {
            _startingAsteroids.Add(newAsteroid.Id, newAsteroid);
            newAsteroid.AsteroidDestroyed += DeleteAsteroid;
        }

        private void DeleteAsteroid(Asteroid asteroid)
        {
            asteroid.AsteroidDestroyed -= DeleteAsteroid;
            _startingAsteroids.Remove(asteroid.Id, out asteroid);
        }
    }
}