using Abstracts;
using Asteroid;
using System;


namespace Gameplay.Asteroid.Behaviour
{
    public class AsteroidBehaviourController : BaseController
    {
        private readonly AsteroidView _view;
        private AsteroidBehaviour _currentBehaviour;

        public AsteroidBehaviourController(AsteroidView view, AsteroidBehaviourConfig config)
        {
            _view = view;

            _currentBehaviour = CreateAsteroidBehavior(config);
        }

        protected override void OnDispose()
        {
            _currentBehaviour.Dispose();
        }

        private AsteroidBehaviour CreateAsteroidBehavior(AsteroidBehaviourConfig asteroid)
        {
            return asteroid.AsteroidMoveType switch
            {
                AsteroidMoveType.Static => new AsteroidStaticBehavior(_view, asteroid),
                AsteroidMoveType.LinearMotion => new AsteroidLinearMotion(_view, asteroid),
                _ => throw new ArgumentOutOfRangeException(nameof(asteroid.AsteroidMoveType), asteroid.AsteroidMoveType, "A not-existent asteroid behavior type is provided")
            };
        }
    }
}