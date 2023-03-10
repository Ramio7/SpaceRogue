using Gameplay.Asteroid;
using Gameplay.Asteroid.Behaviour;
using Gameplay.Health;
using Gameplay.Player;

public class PlayerBasedAsteroidBehaviourController : AsteroidBehaviourController
{
    public PlayerBasedAsteroidBehaviourController(AsteroidView view,
        AsteroidBehaviourConfig config,
        HealthController healthController,
        PlayerView player) : base(view, config, healthController)
    {
        Player = player;
        CurrentBehaviour = CreateAsteroidBehavior();
    }
}
