using Gameplay.Asteroid;
using Gameplay.Asteroid.Behaviour;
using Gameplay.Mechanics.Timer;
using UnityEngine;

public sealed class AsteroidEscapeMovementBehaviour : AsteroidLinearMotionBehavior
{
    private readonly AsteroidView _creatorView;

    public AsteroidEscapeMovementBehaviour(AsteroidView escapingView, AsteroidBehaviourConfig config, AsteroidView creatorView) : base(escapingView, config)
    {
        _creatorView = creatorView;
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
