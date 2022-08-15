using Gameplay.Enemy.Movement;
using Gameplay.Movement;
using Gameplay.Player;
using Gameplay.Shooting;
using Utilities.Reactive.SubscriptionProperty;

namespace Gameplay.Enemy.Behaviour
{
    public class EnemyCombatBehaviour : EnemyBehaviour
    {
        private readonly MovementModel _movementModel;
        private readonly EnemyInputController _inputController;
        private readonly FrontalTurretController _frontalTurret;
        private readonly PlayerView _target;
        
        public EnemyCombatBehaviour(
            SubscribedProperty<EnemyState> enemyState, 
            PlayerView playerView, 
            MovementModel movementModel, 
            EnemyInputController inputController,
            FrontalTurretController frontalTurret) : base(enemyState, playerView)
        {
            _movementModel = movementModel;
            _inputController = inputController;
            _frontalTurret = frontalTurret;
        }

        protected override void OnUpdate()
        {
            //TODO implement hit-and-run tactics
        }
    }
}