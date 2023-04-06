using Gameplay.Space.Generator;
using Zenject;

namespace Gameplay.Asteroids.Factories
{
    public class AsteroidSpawnerFactory : PlaceholderFactory<SpawnPointsFinder, AsteroidSpawner>
    {
        private readonly AsteroidFactory _factory;

        public AsteroidSpawnerFactory(AsteroidFactory factory)
        {
            _factory = factory;
        }

        public override AsteroidSpawner Create(SpawnPointsFinder finder)
        {
            return new(finder, _factory);
        }
    }
}