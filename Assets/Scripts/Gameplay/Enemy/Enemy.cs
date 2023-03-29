using System;
using Gameplay.Movement;
using Gameplay.Survival;

namespace Gameplay.Enemy
{
    public sealed class Enemy : IDisposable
    {
        private readonly UnitMovement _unitMovement;
        private readonly UnitTurning _unitTurning;

        public EnemyView EnemyView { get; }
        public EntitySurvival Survival { get; }

        public Enemy(
            EnemyView enemyView, 
            UnitMovement unitMovement,
            UnitTurning unitTurning,
            EntitySurvival enemySurvival)
        {
            EnemyView = enemyView;
            _unitMovement = unitMovement;
            _unitTurning = unitTurning;
            Survival = enemySurvival;

            Survival.EntityHealth.HealthReachedZero += OnDeath;
        }

        public void Dispose()
        {
            Survival.EntityHealth.HealthReachedZero -= OnDeath;

            Survival.Dispose();
            _unitMovement.Dispose();
            _unitTurning.Dispose();

            UnityEngine.Object.Destroy(EnemyView.gameObject);
        }

        private void OnDeath()
        {
            Dispose();
        }
    }
}