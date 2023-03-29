using System;
using UI.Game;
using Gameplay.Enemy;
using UnityEngine;

namespace UI.Services
{
    public sealed class EnemyStatusBarService : IDisposable
    {
        private readonly EnemyHealthBarsView _barsView;
        private readonly EnemyFactory _enemyFactory;
        private readonly EnemyStatusBarViewFactory _statusBarViewFactory;
        private readonly FloatStatusBarFactory _floatStatusBarFactory;

        public EnemyStatusBarService(
            EnemyHealthBarsView barsView, 
            EnemyFactory enemyFactory, 
            EnemyStatusBarViewFactory statusBarViewFactory,
            FloatStatusBarFactory floatStatusBarFactory)
        {
            _barsView = barsView;
            _enemyFactory = enemyFactory;
            _statusBarViewFactory = statusBarViewFactory;
            _floatStatusBarFactory = floatStatusBarFactory;
            _enemyFactory.EnemyCreated += OnEnemyCreated;
        }

        public void Dispose()
        {
            _enemyFactory.EnemyCreated -= OnEnemyCreated;
        }

        private void OnEnemyCreated(Enemy enemy)
        {
            var statusBarView = _statusBarViewFactory.Create(enemy.Survival, _barsView);

            if (enemy.EnemyView.TryGetComponent(out Collider2D collider))
            {
                _floatStatusBarFactory.Create(statusBarView, collider, enemy.Survival); 
            }
        }
    }
}