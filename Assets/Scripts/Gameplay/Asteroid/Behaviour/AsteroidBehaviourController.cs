using Abstracts;
using Asteroid;
using Gameplay.Health;
using Gameplay.Player;
using System;
using UnityEngine;

namespace Gameplay.Asteroid.Behaviour
{
    public class AsteroidBehaviourController : BaseController
    {
        protected AsteroidView View;
        protected AsteroidBehaviourConfig Config;
        protected HealthController HealthController;
        protected AsteroidBehaviour CurrentBehaviour;

        protected Collision2D Collision;
        protected PlayerView Player;
        protected AsteroidView CreatorView;

        public AsteroidBehaviourController(AsteroidView view, AsteroidBehaviourConfig config, HealthController healthController)
        {
            View = view;
            Config = config;
            HealthController = healthController;
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
                AsteroidMoveType.RandomDirectedMotion => new AsteroidRandomDirectionMotionBehaviour(View, Config, HealthController),
                AsteroidMoveType.PlayerTargeting => new AsteroidPlayerDirectedMotionBehavior(View, Config, HealthController, Player),
                AsteroidMoveType.CollisionEscaping => new AsteroidEscapeMovementBehaviour(View, Config, HealthController, Collision.transform.position),
                AsteroidMoveType.CreatorEscaping => new AsteroidEscapeMovementBehaviour(View, Config, HealthController, CreatorView.transform.position),
                _ => throw new ArgumentOutOfRangeException(nameof(Config.AsteroidMoveType), Config.AsteroidMoveType, "A not-existent asteroid behavior type is provided")
            };
        }
    }
}