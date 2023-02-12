using Abstracts;
using Asteroid;
using Gameplay.Player;
using System;


namespace Gameplay.Asteroid.Behaviour
{
    public class AsteroidBehaviourController : BaseController
    {
        private readonly AsteroidView _view;
        private AsteroidBehaviour _currentBehaviour;

        public AsteroidBehaviourController(AsteroidView view, AsteroidBehaviourConfig config, PlayerView player)
        {
            _view = view;

            _currentBehaviour = CreateAsteroidBehavior(config, player);
        }

        protected override void OnDispose()
        {
            _currentBehaviour.Dispose();
        }

        private AsteroidBehaviour CreateAsteroidBehavior(AsteroidBehaviourConfig asteroid, PlayerView player)
        {
            return asteroid.AsteroidMoveType switch
            {
                AsteroidMoveType.Static => new AsteroidStaticBehavior(_view, asteroid),
                AsteroidMoveType.LinearMotion => new AsteroidLinearMotionBehavior(_view, asteroid),
                AsteroidMoveType.PlayerTargeting => new AsteroidPlayerDirectedMotionBehavior(_view, asteroid, player),
                _ => throw new ArgumentOutOfRangeException(nameof(asteroid.AsteroidMoveType), asteroid.AsteroidMoveType, "A not-existent asteroid behavior type is provided")
            };
        }
    }
}