using Gameplay.Asteroid;
using Gameplay.Asteroid.Behaviour;
using Gameplay.Mechanics.Timer;
using UnityEngine;

public class AsteroidEscapeMovementBehaviour : AsteroidLinearMotionBehavior
{
    private readonly Timer _timer;

    public AsteroidEscapeMovementBehaviour(AsteroidView escapingView, AsteroidBehaviourConfig config, AsteroidView creatorView) : base(escapingView, config)
    {
        _asteroidDirection = SetDirection(creatorView);
        _timer = new(config.AsteroidLifeTime);
        _timer.Start();
    }

    protected override void OnUpdate()
    {
        Move(_asteroidDirection, _speed);

        if (_timer.IsExpired && _config.AsteroidLifeTime != 0)
        {
            Object.Destroy(_view.gameObject);
            _timer.Dispose();
            Dispose();
        }
    }

    private Vector2 SetDirection(AsteroidView creatorView)
    {

        return _view.transform.position;
    }
}
