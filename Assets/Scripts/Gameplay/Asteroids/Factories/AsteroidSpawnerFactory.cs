using Gameplay.Space.Generator;
using Zenject;

namespace Gameplay.Asteroids.Factories
{
    public class AsteroidSpawnerFactory : PlaceholderFactory<SpawnPointsFinder, AsteroidSpawner>
    {
        public override AsteroidSpawner Create(SpawnPointsFinder finder)
        {
            return base.Create(finder);
        }
    }
}