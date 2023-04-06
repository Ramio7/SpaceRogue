using Gameplay.Asteroids.Scriptables;
using Gameplay.Survival;
using UnityEngine;
using Zenject;

namespace Gameplay.Asteroids.Factories
{
    public class AsteroidFactory : PlaceholderFactory<Vector2, AsteroidConfig, Asteroid>
    {
        private readonly AsteroidViewFactory _asteroidViewFactory;
        private readonly EntitySurvivalFactory _entitySurvivalFactory;

        public AsteroidFactory(
            AsteroidViewFactory asteroidViewFactory,
            EntitySurvivalFactory entitySurvivalFactory)
        {
            _asteroidViewFactory = asteroidViewFactory;
            _entitySurvivalFactory = entitySurvivalFactory;
        }

        public override Asteroid Create(Vector2 spawnPoint, AsteroidConfig config)
        {
            var view = _asteroidViewFactory.Create(spawnPoint, config);
            var survival = _entitySurvivalFactory.Create(view, config.SurvivalConfig);
            return new(view, survival);
        }
    }
}