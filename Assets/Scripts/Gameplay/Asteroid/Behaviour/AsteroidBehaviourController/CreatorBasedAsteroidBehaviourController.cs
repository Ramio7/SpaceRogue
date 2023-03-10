using Gameplay.Asteroid;
using Gameplay.Asteroid.Behaviour;
using Gameplay.Health;

public class CreatorBasedAsteroidBehaviourController : AsteroidBehaviourController
{
    public CreatorBasedAsteroidBehaviourController(
        AsteroidView escapingView,
        AsteroidBehaviourConfig config,
        HealthController healthController,
        AsteroidView creatorView) : base(escapingView, config, healthController)
    {
        CreatorView = creatorView;
        CurrentBehaviour = CreateAsteroidBehavior();
    }
}
