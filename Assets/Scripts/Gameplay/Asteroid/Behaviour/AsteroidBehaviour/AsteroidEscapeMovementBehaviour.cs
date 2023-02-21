using Gameplay.Asteroid;
using Gameplay.Asteroid.Behaviour;
using UnityEngine;

public sealed class AsteroidEscapeMovementBehaviour : AsteroidLinearMotionBehaviorBase
{
    private readonly AsteroidView _creatorView;

    public AsteroidEscapeMovementBehaviour(AsteroidView escapingView, AsteroidBehaviourConfig config, AsteroidView creatorView) : base(escapingView, config)
    {
        _creatorView = creatorView;
        AsteroidStart();
    }

    protected override void AsteroidStart()
    {
        AsteroidDirection = SetDirection(_creatorView);
        Move(AsteroidDirection, Config.AsteroidStartingForce);
    }

    private Vector2 SetDirection(AsteroidView creatorView)
    {
        return View.transform.position - creatorView.transform.position;
    }
}
