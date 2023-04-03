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
        private readonly RandomDirectedAsteroidFactory _randomDirectedAsteroidFactory;
        private readonly TargetedAsteroidFactory _targetedAsteroidFactory;
        private readonly TimerFactory _timerFactory;
        private readonly PlayerLocator _playerLocator;

        public AsteroidsInSpaceFactory(
            AsteroidSpawnConfig config, 
            RandomDirectedAsteroidFactory asteroidFactory, 
            TargetedAsteroidFactory targetedAsteroidFactory, 
            TimerFactory timerFactory,
            PlayerLocator playerLocator)
        {
            _config = config;
            _randomDirectedAsteroidFactory = asteroidFactory;
            _targetedAsteroidFactory = targetedAsteroidFactory;
            _timerFactory = timerFactory;
            _playerLocator = playerLocator;
        }

        public override AsteroidsInSpace Create(int asteroidsSpawnOnStartCount, SpawnPointsFinder spawnPointsFinder)
        {
            return new(asteroidsSpawnOnStartCount, _config, 
                spawnPointsFinder, 
                _randomDirectedAsteroidFactory, 
                _targetedAsteroidFactory, 
                _timerFactory,
                _playerLocator);
        }
    }
}