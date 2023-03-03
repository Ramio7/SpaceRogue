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
        protected AsteroidBehaviourConfig Config;
        protected AsteroidBehaviour CurrentBehaviour;

        protected Collision2D Collision;
        protected PlayerView Player;
        protected AsteroidView CreatorView;

        public AsteroidBehaviourController(AsteroidView view, AsteroidBehaviourConfig config)
        {
            View = view;
            Config = config;
        }

        protected override void OnDispose()
        {
            CurrentBehaviour.Dispose();
            Config = null;
        }

        protected AsteroidBehaviour CreateAsteroidBehavior()
        {
            return Config.AsteroidMoveType switch
            {
                AsteroidMoveType.Static => new AsteroidStaticBehavior(View, Config),
                AsteroidMoveType.LinearMotion => new AsteroidLinearMotionBehavior(View, Config),
                AsteroidMoveType.PlayerTargeting => new AsteroidPlayerDirectedMotionBehavior(View, Config, Player),
                AsteroidMoveType.CollisionEscaping => new AsteroidCollisionCounterDirectedMotionBehavior(View, Config, Collision),
                AsteroidMoveType.CreatorEscaping => new AsteroidEscapeMovementBehaviour(View, Config, CreatorView),
                _ => throw new ArgumentOutOfRangeException(nameof(Config.AsteroidMoveType), Config.AsteroidMoveType, "A not-existent asteroid behavior type is provided")
            };
        }
    }
}