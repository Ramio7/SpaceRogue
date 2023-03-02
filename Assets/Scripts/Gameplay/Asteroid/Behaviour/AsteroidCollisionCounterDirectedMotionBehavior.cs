using Gameplay.Asteroid;
using Gameplay.Asteroid.Behaviour;
using UnityEngine;

public sealed class AsteroidCollisionCounterDirectedMotionBehavior : AsteroidLinearMotionBehavior
{
    private readonly Collision2D _collision;

    public AsteroidCollisionCounterDirectedMotionBehavior(AsteroidView view, AsteroidBehaviourConfig config, Collision2D collision) : base(view, config)
    {
        _collision = collision;
    }

    protected override void AsteroidStart()
    {
        AsteroidDirection = SetDirection(_collision);
        Move(AsteroidDirection, Config.AsteroidStartingForce);
    }

    private Vector2 SetDirection(Collision2D collision)
    {
        if (Config.SpawnRadius != 0) return View.transform.position + (Random.insideUnitSphere * Config.SpawnRadius) - collision.transform.position;
        return View.transform.position + Random.insideUnitSphere - collision.transform.position;
    }
}
