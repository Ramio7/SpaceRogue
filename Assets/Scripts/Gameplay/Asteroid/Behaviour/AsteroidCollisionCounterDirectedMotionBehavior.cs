using Gameplay.Asteroid;
using Gameplay.Asteroid.Behaviour;
using Gameplay.Mechanics.Timer;
using UnityEngine;

public sealed class AsteroidCollisionCounterDirectedMotionBehavior : AsteroidLinearMotionBehavior
{
    private readonly Timer _timer;

    public AsteroidCollisionCounterDirectedMotionBehavior(AsteroidView view, AsteroidBehaviourConfig config, Collision2D collision) : base(view, config)
    {
        var asteroidDirection = SetDirection(collision);
        _timer = new(config.AsteroidLifeTime);
        _timer.Start();
        Move(asteroidDirection, config.AsteroidStartingForce);
    }

    protected override void OnUpdate()
    {
        if (_timer.IsExpired && _config.AsteroidLifeTime != 0)
        {
            Object.Destroy(_view.gameObject);
            _timer.Dispose();
            Dispose();
        }
    }

    private Vector2 SetDirection(Collision2D collision)
    {
        if (_config.SpawnRadius != 0) return _view.transform.position + (Random.insideUnitSphere * _config.SpawnRadius) - collision.transform.position;
        return _view.transform.position + Random.insideUnitSphere - collision.transform.position;
    }
}
