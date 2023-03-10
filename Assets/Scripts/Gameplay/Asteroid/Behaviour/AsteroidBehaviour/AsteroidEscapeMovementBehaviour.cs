using Gameplay.Asteroid;
using Gameplay.Asteroid.Behaviour;
using Gameplay.Health;
using UnityEngine;

public sealed class AsteroidEscapeMovementBehaviour : AsteroidLinearMotionBehaviorBase
{
    private readonly Vector2 _escapingPosition;

    public AsteroidEscapeMovementBehaviour(AsteroidView escapingView, AsteroidBehaviourConfig config, HealthController healthController, Vector2 escapingPosition) : base(escapingView, config, healthController)
    {
        _escapingPosition = escapingPosition;
        AsteroidStart();
    }

    protected override void AsteroidStart()
    {
        AsteroidDirection = SetDirection(_escapingPosition);
        Move(AsteroidDirection, Config.AsteroidStartingForce);
    }

    private Vector2 SetDirection(Vector2 escapingPosition) => View.transform.position - (Vector3)escapingPosition;
}
