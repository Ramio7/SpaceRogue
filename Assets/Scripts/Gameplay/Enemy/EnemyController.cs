using Abstracts;
using Gameplay.Enemy.Behaviour;
using Gameplay.Movement;
using Gameplay.Player;
using Gameplay.Shooting;
using Gameplay.Enemy.Scriptables;
using UnityEngine;

namespace Gameplay.Enemy
{
    public sealed class EnemyController : BaseController
    {
        public EnemyView View => _view;

        private readonly EnemyView _view;
        private readonly LegacyEnemyConfig _config;
        private readonly Weapon _turret;
        private readonly EnemyBehaviourController _behaviourController;
        private readonly PlayerController _playerController;

        public EnemyController(LegacyEnemyConfig config, EnemyView view, PlayerController playerController, Transform target)
        {
            _playerController = playerController;
            _config = config;
            _view = view;
            AddGameObject(_view.gameObject);
            //_turret = WeaponFactory.CreateFrontalTurret(PickTurret(_config.TurretConfigs, _random), _view.transform, UnitType.Enemy);
            //AddController(_turret);
            
            var movementModel = new UnitMovementModel(_config.UnitMovement);
            _behaviourController = new(movementModel, _view, _turret, _playerController, _config.Behaviour, target);
            AddController(_behaviourController);
        }
    }
}