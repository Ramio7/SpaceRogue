using Gameplay.Player;
using Scriptables.Asteroid;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Mathematics;
using Utilities.Unity;

namespace Gameplay.Asteroid
{
    public class AsteroidFactory
    {
        private GameObject _pool;
        private PlayerView _player;
        const int _maxSpawnTries = 10;

        public AsteroidFactory(GameObject pool, PlayerView player)
        {
            _pool = pool;
            _player = player;
        }

        public AsteroidController CreateAsteroid(Vector3 spawnPosition, SingleAsteroidConfig config) => new(config, CreateAsteroidView(spawnPosition, config), _player);

        public AsteroidController CreateAsteroid(Vector3 spawnPosition, SingleAsteroidConfig config, GameObject pool) =>
            new(config, CreateAsteroidView(spawnPosition, config, pool), _player);

        public AsteroidController CreateAsteroidOnRadius(Vector3 spawnPoint, SingleAsteroidConfig config)
        {
            var spawnPosition = UnityHelper.GetAPointOnRadius(spawnPoint, config.Behaviour.SpawnRadius);

            int spawnTries = 0;
            while (UnityHelper.IsAnyObjectAtPosition(spawnPosition, config.Size.AsteroidScale.MaxVector3CoordinateOnPlane()) && spawnTries <= _maxSpawnTries)
            {
                spawnPosition = UnityHelper.GetAPointOnRadius(spawnPoint, config.Behaviour.SpawnRadius);
                spawnTries++;
            }

            if (spawnTries > _maxSpawnTries) return null;
            return CreateAsteroid(spawnPosition, config);
        }

        public AsteroidController CreateAsteroidInsideRadius(Vector3 spawnPoint, SingleAsteroidConfig config)
        {
            var spawnPosition = (Vector2)spawnPoint + Random.insideUnitCircle * config.Behaviour.SpawnRadius;

            int spawnTries = 0;
            while (UnityHelper.IsAnyObjectAtPosition(spawnPosition, config.Size.AsteroidScale.MaxVector3CoordinateOnPlane()) && spawnTries <= _maxSpawnTries)
            {
                spawnPosition = (Vector2)spawnPoint + Random.insideUnitCircle * config.Behaviour.SpawnRadius;
                spawnTries++;
            }

            if (spawnTries > _maxSpawnTries) return null;
            return CreateAsteroid(spawnPosition, config, _pool);
        }

        public AsteroidController CreateAsteroidInsideCloudRadius(Vector3 spawnPoint, SingleAsteroidConfig config, GameObject pool, Vector3 asteroidCloudSize)
        {
            var spawnRadius = asteroidCloudSize.MaxVector3CoordinateOnPlane() / 2;
            var spawnPosition = (Vector2)spawnPoint + Random.insideUnitCircle * spawnRadius;

            int spawnTries = 0;
            while (UnityHelper.IsAnyObjectAtPosition(spawnPosition, config.Size.AsteroidScale.MaxVector3CoordinateOnPlane()) && spawnTries <= _maxSpawnTries)
            {
                spawnPosition = (Vector2)spawnPoint + Random.insideUnitCircle * spawnRadius;
                spawnTries++;
            }

            if (spawnTries > _maxSpawnTries) return null;
            return CreateAsteroid(spawnPosition, config, pool);
        }

        public List<AsteroidController> CreateAsteroidCloud(Vector3 spawnPosition, AsteroidCloudConfig config)
        {
            var asteroidCloudPool = new GameObject("Asteroid Cloud");
            asteroidCloudPool.transform.SetParent(_pool.transform, false);

            var asteroidControllersOutput = new List<AsteroidController>();
            var asteroidsInCloud = Random.Range(config.MinAsteroidsInCloud, config.MaxAsteroidsInCloud + 1);

            int spawnTries = 0;
            while (asteroidControllersOutput.Count < asteroidsInCloud && spawnTries <= _maxSpawnTries)
            {
                var random = new System.Random();
                var currentConfig = Random.Range(0, config.CloudAsteroidsConfigs.Count);

                if (RandomPicker.TakeChance(config.CloudAsteroidsConfigs[currentConfig].SpawnChance, random))
                {
                    var currentAsteroid = CreateAsteroidInsideCloudRadius(spawnPosition, config.CloudAsteroidsConfigs[currentConfig], asteroidCloudPool, config.AsteroidCloudSize);
                    if (currentAsteroid == null) spawnTries++;
                    asteroidControllersOutput.Add(currentAsteroid);
                }
            }

            return asteroidControllersOutput;
        }

        public AsteroidController CreateAsteroidNearPlayer(SingleAsteroidConfig config) => CreateAsteroidOnRadius(_player.transform.position, config);

        private AsteroidView CreateAsteroidView(Vector3 spawnPosition, SingleAsteroidConfig config)
        {
            var asteroid = Object.Instantiate(config.Prefab, spawnPosition, Quaternion.identity);
            asteroid.transform.name = config.Size.SizeType.ToString() + config.AsteroidType.ToString();
            asteroid.transform.SetParent(_pool.transform, true);
            asteroid.transform.localScale = new Vector3(config.Size.AsteroidScale.x, config.Size.AsteroidScale.y, asteroid.transform.localScale.z);
            return asteroid;
        }

        private AsteroidView CreateAsteroidView(Vector3 spawnPosition, SingleAsteroidConfig config, GameObject pool)
        {
            var asteroid = Object.Instantiate(config.Prefab, spawnPosition, Quaternion.identity);
            asteroid.transform.name = config.Size.SizeType.ToString() + config.AsteroidType.ToString();
            asteroid.transform.SetParent(pool.transform, true);
            asteroid.transform.localScale = new Vector3(config.Size.AsteroidScale.x, config.Size.AsteroidScale.y, asteroid.transform.localScale.z);
            return asteroid;
        }

        public void Dispose()
        {
            _pool = null;
            _player = null;
        }
    }
}
