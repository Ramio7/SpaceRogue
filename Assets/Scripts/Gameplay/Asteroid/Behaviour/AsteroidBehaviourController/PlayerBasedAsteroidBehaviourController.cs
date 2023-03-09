using Gameplay.Asteroid;
using Gameplay.Asteroid.Behaviour;
using Gameplay.Player;

public class PlayerBasedAsteroidBehaviourController : AsteroidBehaviourController
{
    public PlayerBasedAsteroidBehaviourController(AsteroidView view, AsteroidBehaviourConfig config, PlayerView player) : base(view, config)
    {
        Player = player;
        CurrentBehaviour = CreateAsteroidBehavior();
    }
}
