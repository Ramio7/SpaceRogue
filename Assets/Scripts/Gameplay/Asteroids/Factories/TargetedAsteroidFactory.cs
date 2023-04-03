using Gameplay.Asteroids.Scriptables;
using Gameplay.Asteroids;
using UnityEngine;
using Zenject;
using Gameplay.Asteroids.Factories;
using Gameplay.Survival;

public class TargetedAsteroidFactory : PlaceholderFactory<Vector2, AsteroidConfig, Vector2, Asteroid>
{
    private readonly AsteroidViewFactory _asteroidViewFactory;
    private readonly AsteroidTargetedMovementFactory _asteroidTargetedMovementFactory;
    private readonly EntitySurvivalFactory _entitySurvivalFactory;

    public TargetedAsteroidFactory(
        AsteroidViewFactory asteroidViewFactory, 
        AsteroidTargetedMovementFactory asteroidTargetedMovementFactory, 
        EntitySurvivalFactory entitySurvivalFactory)
    {
        _asteroidViewFactory = asteroidViewFactory;
        _asteroidTargetedMovementFactory = asteroidTargetedMovementFactory;
        _entitySurvivalFactory = entitySurvivalFactory;
    }

    public override Asteroid Create(Vector2 spawnPoint, AsteroidConfig config, Vector2 targetPosition)
    {
        var view = _asteroidViewFactory.Create(spawnPoint, config);
        var movement = _asteroidTargetedMovementFactory.Create(config.StartingSpeed, view, targetPosition, config.DirectionScattering);
        var survival = _entitySurvivalFactory.Create(view, config.SurvivalConfig);
        return new(view, movement, survival);
    }
}
