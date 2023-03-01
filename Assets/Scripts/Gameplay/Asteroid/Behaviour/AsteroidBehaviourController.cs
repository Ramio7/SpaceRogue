using Abstracts;
using Asteroid;
using Gameplay.Player;
using System;
using UnityEngine;

namespace Gameplay.Asteroid.Behaviour
{
    public class AsteroidBehaviourController : BaseController
    {
        protected AsteroidView View;
        protected AsteroidBehaviour CurrentBehaviour;

        protected Collision2D Collision;
        protected PlayerView Player;
        protected AsteroidView CreatorView;

        public AsteroidBehaviourController(AsteroidView view, AsteroidBehaviourConfig config)
        {
            View = view;
            CurrentBehaviour = CreateAsteroidBehavior(config);
        }

        protected override void OnDispose()
        {
            CurrentBehaviour.Dispose();
        }

        protected AsteroidBehaviour CreateAsteroidBehavior(AsteroidBehaviourConfig asteroid)
        {
            return asteroid.AsteroidMoveType switch
            {
                AsteroidMoveType.Static => new AsteroidStaticBehavior(View, asteroid),
                AsteroidMoveType.LinearMotion => new AsteroidLinearMotionBehavior(View, asteroid),
                AsteroidMoveType.PlayerTargeting => new AsteroidPlayerDirectedMotionBehavior(View, asteroid, Player),
                AsteroidMoveType.CollisionEscaping => new AsteroidCollisionCounterDirectedMotionBehavior(View, asteroid, Collision),
                AsteroidMoveType.CreatorEscaping => new AsteroidEscapeMovementBehaviour(View, asteroid, CreatorView),
                _ => throw new ArgumentOutOfRangeException(nameof(asteroid.AsteroidMoveType), asteroid.AsteroidMoveType, "A not-existent asteroid behavior type is provided")
            };
        }
    }
}