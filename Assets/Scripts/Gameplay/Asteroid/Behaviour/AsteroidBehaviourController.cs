using Abstracts;
using Asteroid;
using Gameplay.Player;
using System;
using UnityEngine;

namespace Gameplay.Asteroid.Behaviour
{
    public class AsteroidBehaviourController : BaseController
    {
        private readonly AsteroidView _view;
        private readonly Collision2D _collision;
        private readonly AsteroidBehaviour _currentBehaviour;
        private readonly PlayerView _player;
        private readonly AsteroidView _creatorView;

        public AsteroidBehaviourController(AsteroidView view, AsteroidBehaviourConfig config, PlayerView player)
        {
            _view = view;
            _player = player;
            _currentBehaviour = CreateAsteroidBehavior(config);
        }

        public AsteroidBehaviourController(AsteroidView escapingView, AsteroidBehaviourConfig config, AsteroidView creatorView)
        {
            _view = escapingView;
            _creatorView = creatorView;
            _currentBehaviour = CreateAsteroidBehavior(config);
        }

        public AsteroidBehaviourController(AsteroidView view, AsteroidBehaviourConfig config, Collision2D collision)
        {
            _view = view;
            _collision = collision;
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
                AsteroidMoveType.LinearMotion => new AsteroidLinearMotionBehavior(_view, asteroid),
                AsteroidMoveType.PlayerTargeting => new AsteroidPlayerDirectedMotionBehavior(_view, asteroid, _player),
                AsteroidMoveType.CollisionEscaping => new AsteroidCollisionCounterDirectedMotionBehavior(_view, asteroid, _collision),
                AsteroidMoveType.CreatorEscaping => new AsteroidEscapeMovementBehaviour(_view, asteroid, _creatorView),
                _ => throw new ArgumentOutOfRangeException(nameof(asteroid.AsteroidMoveType), asteroid.AsteroidMoveType, "A not-existent asteroid behavior type is provided")
            };
        }
    }
}