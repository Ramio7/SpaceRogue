using Abstracts;
using Gameplay.Asteroid.Behaviour;
using Gameplay.Damage;
using Gameplay.Health;
using Gameplay.Player;
using Scriptables.Asteroid;
using Scriptables.Health;
using System;

namespace Gameplay.Asteroid
{
    public sealed class AsteroidController : BaseController
    {
        public Action OnDestroy { get; private set; }

        private readonly AsteroidView _view;
        private readonly AsteroidConfig _config;
        private readonly AsteroidBehaviourController _behaviourController;

        public AsteroidController(AsteroidConfig config, AsteroidView view, PlayerView player)
        {
            _config = config;

            _view = view;
            AddGameObject(_view.gameObject);

            var damageModel = new DamageModel(config.CollisionDamageAmount);
            _view.Init(damageModel);

            _behaviourController = new AsteroidBehaviourController(view, _config.Behaviour, player);
            AddController(_behaviourController);

            AddHealthController(_config.Health);
        }

        protected override void OnDispose()
        {
            _behaviourController.Dispose();
        }


        private HealthController AddHealthController(HealthConfig healthConfig)
        {
            var healthController = new HealthController(healthConfig, _view);

            healthController.SubscribeToOnDestroy(Dispose);

            AddController(healthController);

            return healthController;
        }

    }
}