using Abstracts;
using Gameplay.Asteroid.Behaviour;
using Gameplay.Damage;
using Gameplay.Health;
using Scriptables.Asteroid;
using Scriptables.Health;

namespace Gameplay.Asteroid
{
    public sealed class AsteroidController : BaseController
    {
        private readonly AsteroidView _view;
        private readonly AsteroidConfig _config;
        private readonly AsteroidBehaviourController _behaviourController;
        private readonly HealthController _healthController;

        public AsteroidController(AsteroidConfig config, AsteroidView view)
        {
            _config = config;

            _view = view;
            AddGameObject(_view.gameObject);

            var damageModel = new DamageModel(config.CollisionDamageAmount);
            _view.Init(damageModel);

            _behaviourController = new AsteroidBehaviourController(view, _config.Behaviour);
            AddController(_behaviourController);

            AddHealthController(_config.Health);
        }

        protected override void OnDispose()
        {
            _behaviourController.Dispose();
            _healthController.Dispose();
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