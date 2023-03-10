using Gameplay.Asteroid;
using Gameplay.Asteroid.Behaviour;
using Gameplay.Health;
using UnityEngine;

public class CollisionBasedAsteroidBehaviourController : AsteroidBehaviourController
{
    public CollisionBasedAsteroidBehaviourController(
        AsteroidView view,
        AsteroidBehaviourConfig config,
        HealthController healthController,
        Collision2D collision) : base(view, config, healthController)
    {
        Collision = collision;
        CurrentBehaviour = CreateAsteroidBehavior();
    }
}
