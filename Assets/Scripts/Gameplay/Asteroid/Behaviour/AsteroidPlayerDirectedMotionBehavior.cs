using Gameplay.Asteroid;
using Gameplay.Asteroid.Behaviour;
using Gameplay.Mechanics.Timer;
using Gameplay.Player;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class AsteroidPlayerDirectedMotionBehavior : AsteroidLinearMotionBehavior
{
    private readonly Timer _timer;

    public AsteroidPlayerDirectedMotionBehavior(AsteroidView view, AsteroidBehaviourConfig config, PlayerView player) : base(view, config)
    {
        var asteroidDirection = SetDirection(player);
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

    private Vector2 SetDirection(PlayerView player)
    {
        var destination = player.transform.position + Random.insideUnitSphere * _config.TargetDispersion;
        return destination - _view.transform.position;
    }
}
