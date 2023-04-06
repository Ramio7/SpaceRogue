using Gameplay.Asteroids.Scriptables;
using Gameplay.Mechanics.Timer;
using Gameplay.Services;
using Gameplay.Space.Generator;
using Zenject;

namespace Gameplay.Asteroids.Factories
{
    public class AsteroidsInSpaceFactory : PlaceholderFactory<int, SpawnPointsFinder, AsteroidsInSpace>
    {
        private readonly AsteroidSpawnConfig _config;
        private readonly TimerFactory _timerFactory;
        private readonly PlayerLocator _playerLocator;
        private readonly AsteroidSpawnerFactory _spawnerFactory;

        public AsteroidsInSpaceFactory(
            AsteroidSpawnConfig config,
            TimerFactory timerFactory,
            PlayerLocator playerLocator,
            AsteroidSpawnerFactory spawnerFactory)
        {
            _config = config;
            _timerFactory = timerFactory;
            _playerLocator = playerLocator;
            _spawnerFactory = spawnerFactory;
        }

        public override AsteroidsInSpace Create(int asteroidsSpawnOnStartCount, SpawnPointsFinder spawnPointsFinder)
        {
            return new(asteroidsSpawnOnStartCount,
                _config,
                _timerFactory,
                _playerLocator,
                _spawnerFactory.Create(spawnPointsFinder));
        }
    }
}