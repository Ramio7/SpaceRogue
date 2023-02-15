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
        public Action<AsteroidController> OnDestroy;
        
        public AsteroidView View { get => _view; }
        public SingleAsteroidConfig Config { get => _config; }

        private SingleAsteroidConfig _config;
        private readonly AsteroidView _view;
        private readonly AsteroidBehaviourController _behaviourController;
        private readonly HealthController _healthController;

        public AsteroidController(SingleAsteroidConfig config, AsteroidView view, PlayerView player)
        {
            _config = config;

            _view = view;
            AddGameObject(_view.gameObject);

            var damageModel = new DamageModel(config.CollisionDamageAmount);
            _view.Init(damageModel);

            _behaviourController = new AsteroidBehaviourController(view, _config.Behaviour, player);
            AddController(_behaviourController);

            _healthController = AddHealthController(_config.Health);
        }

        public AsteroidController(SingleAsteroidConfig config, AsteroidView view, Collision2D collision)
        {
            _config = config;

            _view = view;
            AddGameObject(_view.gameObject);

            var damageModel = new DamageModel(config.CollisionDamageAmount);
            _view.Init(damageModel);

            _behaviourController = new AsteroidBehaviourController(view, _config.Behaviour, collision);
            AddController(_behaviourController);

            _healthController = AddHealthController(_config.Health);
        }

        protected override void OnDispose()
        {
            _config = null;
            _behaviourController?.Dispose();
            _healthController?.Dispose();
            OnDestroy?.Invoke(this);
        }

        private HealthController AddHealthController(HealthConfig healthConfig)
        {
            var healthController = new HealthController(healthConfig, _view);

            healthController.SubscribeToOnDestroy(OnDispose);
            healthController.SubscribeToOnDestroy(Dispose);

            AddController(healthController);

            return healthController;
        }
    }
}