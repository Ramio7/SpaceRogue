using Gameplay.Asteroids.Scriptables;
using Gameplay.Survival;
using UnityEngine;
using Zenject;

namespace Gameplay.Asteroids.Factories
{
    public class AsteroidFactory : PlaceholderFactory<Vector2, AsteroidConfig, Asteroid>
    {
        private readonly AsteroidViewFactory _asteroidViewFactory;
        private readonly AsteroidMovementFactory _asteroidMovementFactory;
        private readonly EntitySurvivalFactory _entitySurvivalFactory;

        public AsteroidFactory(AsteroidViewFactory asteroidViewFactory, AsteroidMovementFactory asteroidMovementFactory, EntitySurvivalFactory entitySurvivalFactory)
        {
            _asteroidViewFactory = asteroidViewFactory;
            _asteroidMovementFactory = asteroidMovementFactory;
            _entitySurvivalFactory = entitySurvivalFactory;
        }

        public override Asteroid Create(Vector2 spawnPoint, AsteroidConfig config)
        {
            var view = _asteroidViewFactory.Create(spawnPoint, config);
            var movement = _asteroidMovementFactory.Create(config.StartingSpeed, view);
            var survival = _entitySurvivalFactory.Create(view, config.SurvivalConfig);
            return new(view, movement, survival);
        }
    }
}