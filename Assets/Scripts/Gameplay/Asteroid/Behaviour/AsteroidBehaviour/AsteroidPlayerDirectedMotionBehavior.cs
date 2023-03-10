using Gameplay.Asteroid;
using Gameplay.Asteroid.Behaviour;
using Gameplay.Health;
using Gameplay.Player;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class AsteroidPlayerDirectedMotionBehavior : AsteroidLinearMotionBehaviorBase
{
    private readonly PlayerView _playerView;

    public AsteroidPlayerDirectedMotionBehavior(AsteroidView view,
        AsteroidBehaviourConfig config,
        HealthController healthController,
        PlayerView player) : base(view, config, healthController)
    {
        _playerView = player;
        AsteroidStart();
    }

    protected override void AsteroidStart()
    {
        AsteroidDirection = SetDirection(_playerView);
        Move(AsteroidDirection, Config.AsteroidStartingForce);
    }

    private Vector2 SetDirection(PlayerView player)
    {
        var destination = player.transform.position + Random.insideUnitSphere * Config.TargetDispersion;
        return View.transform.position - destination;
    }
}
