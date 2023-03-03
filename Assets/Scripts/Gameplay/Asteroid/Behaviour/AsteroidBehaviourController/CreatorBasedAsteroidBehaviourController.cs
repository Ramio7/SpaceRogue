using Gameplay.Asteroid;
using Gameplay.Asteroid.Behaviour;

public class CreatorBasedAsteroidBehaviourController : AsteroidBehaviourController
{
    public CreatorBasedAsteroidBehaviourController(AsteroidView escapingView, AsteroidBehaviourConfig config, AsteroidView creatorView) : base(escapingView, config)
    {
        CreatorView = creatorView;
        CurrentBehaviour = CreateAsteroidBehavior();
    }
}
