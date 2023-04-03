using Gameplay.Asteroids.Scriptables;
using Gameplay.Survival;
using UnityEngine;
using Zenject;

namespace Gameplay.Asteroids.Factories
{
    public class RandomDirectedAsteroidFactory : PlaceholderFactory<Vector2, AsteroidConfig, Asteroid>
    {
        private readonly AsteroidViewFactory _asteroidViewFactory;
        private readonly AsteroidRandomDirectedMovementFactory _asteroidRandomDirectedMovementFactory;
        private readonly EntitySurvivalFactory _entitySurvivalFactory;

        public RandomDirectedAsteroidFactory(
            AsteroidViewFactory asteroidViewFactory, 
            AsteroidRandomDirectedMovementFactory asteroidRandomDirectedMovementFactory, 
            EntitySurvivalFactory entitySurvivalFactory)
        {
            _asteroidViewFactory = asteroidViewFactory;
            _asteroidRandomDirectedMovementFactory = asteroidRandomDirectedMovementFactory;
            _entitySurvivalFactory = entitySurvivalFactory;
        }

        public override Asteroid Create(Vector2 spawnPoint, AsteroidConfig config)
        {
            var view = _asteroidViewFactory.Create(spawnPoint, config);
            var movement = _asteroidRandomDirectedMovementFactory.Create(config.StartingSpeed, view);
            var survival = _entitySurvivalFactory.Create(view, config.SurvivalConfig);
            return new(view, movement, survival);
        }
    }
}