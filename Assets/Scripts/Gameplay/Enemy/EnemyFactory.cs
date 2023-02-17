using Gameplay.Player;
using Scriptables.Enemy;
using UnityEngine;

namespace Gameplay.Enemy
{
    public sealed class EnemyFactory
    {
        private readonly EnemyConfig _config;
        private readonly GameObject _enemyPool;
        
        public EnemyFactory(EnemyConfig config)
        {
            _config = config;

            _enemyPool = new GameObject("EnemyPool");
            _enemyPool.transform.SetPositionAndRotation(new Vector3(9999, 9999), Quaternion.identity);
        }

        public EnemyController CreateEnemy(Vector3 spawnPosition, PlayerController playerController) 
            => new(_config, CreateEnemyView(spawnPosition), playerController, playerController.View.transform);

        public EnemyController CreateEnemy(Vector3 spawnPosition, PlayerController playerController, Transform target) 
            => new(_config, CreateEnemyView(spawnPosition), playerController, target);

        private EnemyView CreateEnemyView(Vector3 spawnPosition)
        {
            var enemy = Object.Instantiate(_config.Prefab, spawnPosition, Quaternion.identity);
            enemy.transform.SetParent(_enemyPool.transform, true);

            return enemy;
        }
    }
}