using Abstracts;
using Gameplay.Asteroid.Behaviour;
using Gameplay.Damage;
using Gameplay.Health;
using Gameplay.Player;
using Scriptables.Health;
using System;
using UnityEngine;

namespace Gameplay.Asteroid
{
    public sealed class AsteroidController : BaseController
    {
        public int Id;

        public event Action<AsteroidController> OnDestroy;
        
        public AsteroidView View { get; private set; }
        public SingleAsteroidConfig Config { get; private set; }

        private readonly AsteroidBehaviourController _behaviourController;
        private readonly HealthController _healthController;

        public AsteroidController(SingleAsteroidConfig config, AsteroidView view, PlayerView player)
        {
            Config = config;

            View = view;
            AddGameObject(View.gameObject);

            var damageModel = new DamageModel(config.CollisionDamageAmount);
            View.Init(damageModel);

            _behaviourController = new PlayerBasedAsteroidBehaviourController(view, Config.Behaviour, player);
            AddController(_behaviourController);

            _healthController = AddHealthController(Config.Health);
        }

        public AsteroidController(SingleAsteroidConfig config, AsteroidView escapingView, AsteroidView creatorView)
        {
            Config = config;

            View = escapingView;
            AddGameObject(View.gameObject);

            var damageModel = new DamageModel(config.CollisionDamageAmount);
            View.Init(damageModel);

            _behaviourController = new CreatorBasedAsteroidBehaviourController(escapingView, Config.Behaviour, creatorView);
            AddController(_behaviourController);

            _healthController = AddHealthController(Config.Health);
        }

        public AsteroidController(SingleAsteroidConfig config, AsteroidView escapingView, Collision2D collision)
        {
            Config = config;

            View = escapingView;
            AddGameObject(View.gameObject);

            var damageModel = new DamageModel(config.CollisionDamageAmount);
            View.Init(damageModel);

            _behaviourController = new CollisionBasedAsteroidBehaviourController(escapingView, Config.Behaviour, collision);
            AddController(_behaviourController);

            _healthController = AddHealthController(Config.Health);
        }

        protected override void OnDispose()
        {
            _behaviourController?.Dispose();
            _healthController?.Dispose();
            OnDestroy?.Invoke(this);
            Config = null;
        }

        private HealthController AddHealthController(HealthConfig healthConfig)
        {
            var healthController = new HealthController(healthConfig, View);

            healthController.SubscribeToOnDestroy(OnDispose);
            healthController.SubscribeToOnDestroy(Dispose);

            AddController(healthController);

            return healthController;
        }
    }
}