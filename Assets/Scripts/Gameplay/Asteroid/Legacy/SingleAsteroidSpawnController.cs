using Abstracts;
using UnityEngine;
using Gameplay.Player;
using Utilities.Unity;
using Scriptables.Asteroid;
using Utilities.Mathematics;

namespace Gameplay.Asteroid
{
    public class SingleAsteroidSpawnController : BaseController
    {
        private AsteroidFactory _asteroidFactory;
        int _spawnCircleRadius = 50;
        private float _currentAsteroidSpawnTime = 1;
        private AsteroidsSpawnConfig _spawnConfig;
        private float _spawnOffset = 30;
        private readonly AsteroidsSpawnConfig _asteroidSpawnConfig;
        private readonly PlayerView _playerView;
        private readonly AsteroidExplosionController _asteroidExplosionController;
        private readonly CloudAsteroidsSpawnController _cloudAsteroidsSpawnController;

        public SingleAsteroidSpawnController(PlayerView playerView, AsteroidsSpawnConfig asteroidSpawnConfig, AsteroidExplosionController asteroidExplosionController, CloudAsteroidsSpawnController cloudAsteroidsSpawnController)
        {
            _playerView = playerView;
            _asteroidSpawnConfig = asteroidSpawnConfig;
            _spawnConfig = asteroidSpawnConfig;
            _asteroidExplosionController = asteroidExplosionController;
            _cloudAsteroidsSpawnController = cloudAsteroidsSpawnController;

            //if (_spawnConfig.SingleAsteroid.Count != 0 && _spawnConfig.FastAsteroid.Count != 0)
            //{
            //    EntryPoint.SubscribeToUpdate(OnUpdate);
            //}
            //if (asteroidSpawnConfig.SpawnOffset > 20) _spawnOffset = asteroidSpawnConfig.SpawnOffset;
        }

        protected void OnUpdate()
        {
            if (_currentAsteroidSpawnTime<=0.0f)
            {
                //_currentAsteroidSpawnTime = Random.Range(_asteroidSpawnConfig.MinTimeAsteroidSpawn, _asteroidSpawnConfig.MaxTimeAsteroidSpawn);
                AsteroidSpawn();
            }
            CooldownAsteroidSpawn();
        }

        protected void CooldownAsteroidSpawn()
        {
            _currentAsteroidSpawnTime -= Time.deltaTime;
        }

        private void AsteroidSpawn()
        {
            //var spawnConfig = (Random.Range(0, 10) > 5) ? _spawnConfig.SingleAsteroid[Random.Range(0, _spawnConfig.SingleAsteroid.Count)] : _spawnConfig.FastAsteroid[Random.Range(0, _spawnConfig.FastAsteroid.Count)];

            //_asteroidFactory = new AsteroidFactory(spawnConfig);

            //var unitSize = spawnConfig.Prefab.transform.localScale;

            //var unitSpawnPoint = GetEmptySpawnPoint(_playerView.gameObject.transform.position, unitSize, _spawnCircleRadius);

            //var asteroidController = _asteroidFactory.CreateAsteroid(unitSpawnPoint,_playerView, _asteroidExplosionController, _cloudAsteroidsSpawnController);
           
            //AddController(asteroidController);
            
        }

        private Vector3 GetEmptySpawnPoint(Vector3 spawnPoint, Vector3 unitSize, int spawnCircleRadius)
        {
            var unitSpawnPoint = spawnPoint + (Vector3)(Random.insideUnitCircle * spawnCircleRadius);
            float unitMaxSize = unitSize.MaxVector3CoordinateOnPlane();
            if (Random.Range(0, 2) == 0) unitSpawnPoint.x = (spawnPoint.x - unitSpawnPoint.x <= 0) ? unitSpawnPoint.x + _spawnOffset : unitSpawnPoint.x - _spawnOffset;
            else unitSpawnPoint.y = (spawnPoint.y - unitSpawnPoint.y <= 0) ? unitSpawnPoint.y + _spawnOffset : unitSpawnPoint.y - _spawnOffset;

            while (UnityHelper.IsAnyObjectAtPosition(unitSpawnPoint, unitMaxSize))
            {
                unitSpawnPoint = spawnPoint + (Vector3)(Random.insideUnitCircle * spawnCircleRadius);
                if (Random.Range(0, 2) == 0) unitSpawnPoint.x = (spawnPoint.x - unitSpawnPoint.x <= 0) ? unitSpawnPoint.x + _spawnOffset : unitSpawnPoint.x - _spawnOffset;
                else unitSpawnPoint.y = (spawnPoint.y - unitSpawnPoint.y <= 0) ? unitSpawnPoint.y + _spawnOffset : unitSpawnPoint.y - _spawnOffset;
            }

            return unitSpawnPoint;
        }
    }
}