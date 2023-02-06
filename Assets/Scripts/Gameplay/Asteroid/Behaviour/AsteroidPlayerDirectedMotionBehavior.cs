using Gameplay.Asteroid;
using Gameplay.Asteroid.Behaviour;
using Gameplay.Player;
using UnityEngine;

public sealed class AsteroidPlayerDirectedMotionBehavior : AsteroidLinearMotionBehavior
{
    public AsteroidPlayerDirectedMotionBehavior(AsteroidView view, AsteroidBehaviourConfig config, PlayerView player) : base(view, config)
    {
        _asteroidDirection = SetDirection(player);
    }

    protected override void OnUpdate()
    {
        Move(_asteroidDirection, _speed);
    }

    private Vector2 SetDirection(PlayerView player)
    {
        return player.transform.position - _view.transform.position;
    }
}
