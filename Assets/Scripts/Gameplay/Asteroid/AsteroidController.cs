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
        public string Id { get; private set; }

        public event Action<AsteroidController> OnDestroy;
        
        public AsteroidView View { get; private set; }
        public SingleAsteroidConfig Config { get; private set; }
        public Collision2D LastCollision { get; private set; }

        private readonly AsteroidBehaviourController _behaviourController;
        private readonly HealthController _healthController;

        public AsteroidController(SingleAsteroidConfig config, AsteroidView view, PlayerView player)
        {
            Id = Guid.NewGuid().ToString();

            Config = config;

            View = view;
            AddGameObject(View.gameObject);
            View.CollisionEntered += GetCollision;

            var damageModel = new DamageModel(config.CollisionDamageAmount);
            View.Init(damageModel);

            _healthController = AddHealthController(Config.Health);
            View.AsteroidDestroyed += _healthController.DestroyUnit;

            _behaviourController = new PlayerBasedAsteroidBehaviourController(view, Config.Behaviour, _healthController, player);
            AddController(_behaviourController);
        }

        public AsteroidController(SingleAsteroidConfig config, AsteroidView escapingView, AsteroidView creatorView)
        {
            Id = Guid.NewGuid().ToString();

            Config = config;

            View = escapingView;
            AddGameObject(View.gameObject);
            View.CollisionEntered += GetCollision;

            var damageModel = new DamageModel(config.CollisionDamageAmount);
            View.Init(damageModel);

            _healthController = AddHealthController(Config.Health);
            View.AsteroidDestroyed += _healthController.DestroyUnit;

            _behaviourController = new CreatorBasedAsteroidBehaviourController(escapingView, Config.Behaviour, _healthController, creatorView);
            AddController(_behaviourController);
        }

        public AsteroidController(SingleAsteroidConfig config, AsteroidView escapingView, Collision2D collision)
        {
            Id = Guid.NewGuid().ToString();

            Config = config;

            View = escapingView;
            AddGameObject(View.gameObject);
            View.CollisionEntered += GetCollision;

            var damageModel = new DamageModel(config.CollisionDamageAmount);
            View.Init(damageModel);

            _healthController = AddHealthController(Config.Health);
            View.AsteroidDestroyed += _healthController.DestroyUnit;

            _behaviourController = new CollisionBasedAsteroidBehaviourController(escapingView, Config.Behaviour, _healthController, collision);
            AddController(_behaviourController);
        }

        protected override void OnDispose()
        {
            _behaviourController?.Dispose();
            _healthController?.Dispose();
            View.AsteroidDestroyed -= _healthController.DestroyUnit;
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

        private void GetCollision(Collision2D collision) => LastCollision = collision;
    }
}