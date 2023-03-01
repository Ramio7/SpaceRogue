using Gameplay.Asteroid;
using Gameplay.Asteroid.Behaviour;
using UnityEngine;

public class CollisionBasedAsteroidBehaviourController : AsteroidBehaviourController
{
    public CollisionBasedAsteroidBehaviourController(AsteroidView view, AsteroidBehaviourConfig config, Collision2D collision) : base(view, config)
    {
        Collision = collision;
        CurrentBehaviour = CreateAsteroidBehavior(config);
    }

}
