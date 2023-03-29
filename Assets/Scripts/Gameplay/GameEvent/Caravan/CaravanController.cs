using Abstracts;
using Gameplay.Enemy;
using Gameplay.Movement;
using Gameplay.Player;
using Scriptables.GameEvent;
using UnityEngine;
using Utilities.Reactive.SubscriptionProperty;
using Utilities.Unity;

namespace Gameplay.GameEvent.Caravan
{
    public sealed class CaravanController : BaseController
    {
        private readonly BaseCaravanGameEventConfig _baseCaravanGameEvent;
        private readonly PlayerController _playerController;
        private readonly PlayerView _playerView;
        private readonly CaravanView _caravanView;

        public SubscribedProperty<bool> OnDestroy = new();

        public CaravanController(GameEventConfig config, PlayerController playerController, 
            CaravanView caravanView, Vector3 targetPosition)
        {
            var baseCaravanGameEvent = config as BaseCaravanGameEventConfig;
            _baseCaravanGameEvent = baseCaravanGameEvent
                ? baseCaravanGameEvent
                : throw new System.Exception("Wrong config type was provided");

            _playerController = playerController;
            _playerView = _playerController.View;

            _caravanView = caravanView;
            //_caravanView.Init(new(0));
            AddGameObject(_caravanView.gameObject);

            AddCarnavalBehaviourController(_baseCaravanGameEvent.CaravanConfig.UnitMovement, targetPosition);

            AddEnemyGroup(_baseCaravanGameEvent, _caravanView.transform.position, _playerController, _caravanView.transform);
        }

        private void AddCarnavalBehaviourController(UnitMovementConfig unitMovement, Vector3 targetPosition)
        {
            var behaviourController = new CaravanBehaviourController(new UnitMovementModel(unitMovement), _caravanView, targetPosition);
            AddController(behaviourController);
        }

        private void OnCaravanDestroyed()
        {
            if (_playerView == null)
            {
                OnDestroy.Value = true;
                return;
            }

            if (_caravanView.IsLastDamageFromPlayer)
            {
                StandardCaravanDestroyed();
                CaravanTrapDestroyed();
            }

            OnDestroy.Value = true;
        }

        private void StandardCaravanDestroyed()
        {
            var config = _baseCaravanGameEvent as CaravanGameEventConfig;
            
            if(config == null)
            {
                return;
            }

            //_caravanView.Init(new(config.AddHealth, UnitType.Assistant));
            //_playerView.TakeDamage(_caravanView);
        }

        private void CaravanTrapDestroyed()
        {
            var config = _baseCaravanGameEvent as CaravanTrapGameEventConfig;

            if (config == null)
            {
                return;
            }

            Debug($"AlertRadius = {config.AlertRadius}");
        }

        private void AddEnemyGroup(BaseCaravanGameEventConfig config, Vector3 spawnPoint, 
            PlayerController playerController, Transform target)
        {
            var enemyFactory = new LegacyEnemyFactory(config.LegacyEnemyConfig);
            var unitSize = config.LegacyEnemyConfig.Prefab.transform.localScale;
            
            var spawnCircleRadius = config.EnemyCount * 2;
            for (int i = 0; i < config.EnemyCount; i++)
            {
                var unitSpawnPoint = UnityHelper.GetEmptySpawnPoint(spawnPoint, unitSize, spawnCircleRadius);
                var enemyController = enemyFactory.CreateEnemy(unitSpawnPoint, playerController, target);
                AddController(enemyController);
            }
        }
    }
}