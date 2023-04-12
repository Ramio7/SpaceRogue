using Gameplay.Asteroids.Scriptables;
using Gameplay.Mechanics.Timer;
using Gameplay.Services;
using Gameplay.Space.Generator;
using Zenject;

namespace Gameplay.Asteroids.Factories
{
    public class PlayerTargetedAsteroidsSpawnerFactory : PlaceholderFactory<float, SpawnPointsFinder, PlayerTargetedAsteroidSpawner>
    {
        private readonly AsteroidSpawnerFactory _spawner;
        private readonly PlayerLocator _playerLocator;
        private readonly TimerFactory _timerFactory;
        private readonly AsteroidSpawnConfig _config;

        public PlayerTargetedAsteroidsSpawnerFactory(AsteroidSpawnerFactory spawner, PlayerLocator playerLocator, TimerFactory timerFactory, AsteroidSpawnConfig config)
        {
            _spawner = spawner;
            _playerLocator = playerLocator;
            _timerFactory = timerFactory;
            _config = config;
        }

        public override PlayerTargetedAsteroidSpawner Create(float asteroidSpawnDelay, SpawnPointsFinder finder)
        {
            return new(_spawner.Create(finder), _playerLocator, _timerFactory, _config, asteroidSpawnDelay);
        }
    }
}