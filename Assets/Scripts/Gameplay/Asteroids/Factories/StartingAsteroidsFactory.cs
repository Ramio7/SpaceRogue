using Gameplay.Asteroids.Scriptables;
using Gameplay.Space.Generator;
using Zenject;

namespace Gameplay.Asteroids.Factories
{
    public class StartingAsteroidsFactory : PlaceholderFactory<int, SpawnPointsFinder, StartingAsteroids>
    {
        private readonly AsteroidSpawnConfig _config;
        private readonly AsteroidSpawnerFactory _spawnerFactory;

        public StartingAsteroidsFactory(
            AsteroidSpawnConfig config,
            AsteroidSpawnerFactory spawnerFactory)
        {
            _config = config;
            _spawnerFactory = spawnerFactory;
        }

        public override StartingAsteroids Create(int asteroidsSpawnOnStartCount, SpawnPointsFinder spawnPointsFinder)
        {
            return new(asteroidsSpawnOnStartCount,
                _config,
                _spawnerFactory.Create(spawnPointsFinder));
        }
    }
}