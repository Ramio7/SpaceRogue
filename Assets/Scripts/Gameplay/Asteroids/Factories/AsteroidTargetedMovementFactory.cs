using Gameplay.Asteroids;
using UnityEngine;
using Zenject;

public class AsteroidTargetedMovementFactory : PlaceholderFactory<float, AsteroidView, Vector2, float, AsteroidTargetedMovement>
{
    public override AsteroidTargetedMovement Create(float startingSpeed, AsteroidView view, Vector2 targetPosition, float directionScattering)
    {
        var movement = new AsteroidTargetedMovement(view, startingSpeed, targetPosition, directionScattering);
        movement.StartMovement();
        return movement;
    }
}
