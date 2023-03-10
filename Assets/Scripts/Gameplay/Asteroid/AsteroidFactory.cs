using Gameplay.Player;
using Gameplay.Space.Planet;
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
        const int max_spawn_tries = 50;

        public AsteroidFactory(GameObject pool, PlayerView player)
        {
            _pool = pool;
            _player = player;
        }

        public AsteroidFactory(PlanetView planetView)
        {
            _pool = new(planetView.name + "Remains");
        }

        public void Dispose()
        {
            _pool = null;
            _player = null;
        }


        #region MainMethods

        public AsteroidController CreateAsteroid(Vector3 spawnPosition, SingleAsteroidConfig config) => new(config, CreateAsteroidView(spawnPosition, config), _player);
        public AsteroidController CreateAsteroidInPool(Vector3 spawnPosition, SingleAsteroidConfig config, GameObject pool) =>
            new(config, CreateAsteroidView(spawnPosition, config, pool), _player);
        public AsteroidController CreateAsteroidOnAsteroidSelfDestroy(Vector3 spawnPosition, SingleAsteroidConfig config, GameObject pool, AsteroidView creatorView) =>
            new(config, CreateAsteroidView(spawnPosition, config, pool), creatorView);
        public AsteroidController CreateAsteroidAfterCollision(Vector3 spawnPosition, SingleAsteroidConfig config, GameObject pool, Collision2D collision) =>
            new(config, CreateAsteroidView(spawnPosition, config, pool), collision);

        public AsteroidController CreateAsteroidOnRadius(Vector3 spawnPoint, SingleAsteroidConfig config)
        {
            var spawnPosition = new Vector2();
            if (TryGetSpawnPointOnRadius(spawnPoint, config, ref spawnPosition)) return CreateAsteroid(spawnPosition, config);
            else return null;
        }

        public AsteroidController CreateAsteroidInsideRadius(Vector3 spawnPoint, SingleAsteroidConfig config)
        {
            var spawnPosition = (Vector2)spawnPoint + Random.insideUnitCircle * config.Behaviour.SpawnRadius;
            if (TryGetSpawnPointNearSpawnPoint(spawnPoint, config, ref spawnPosition)) return CreateAsteroidInPool(spawnPosition, config, _pool);
            else return null;
        }

        public AsteroidController CreateAsteroidInsideAsteroidCloudAfterDestruction(
            AsteroidView creatorView,
            SingleAsteroidConfig config,
            GameObject pool,
            Vector3 asteroidCloudSize)
        {
            var spawnRadius = asteroidCloudSize.MaxVector3CoordinateOnPlane() / 2;
            var spawnPosition = (Vector2)creatorView.transform.position + Random.insideUnitCircle * spawnRadius;

            if (TryGetSpawnPointIncideAsteroidCloud(spawnPosition, config, spawnRadius, ref spawnPosition)) 
                return CreateAsteroidOnAsteroidSelfDestroy(spawnPosition, config, pool, creatorView);
            else return null;
        }

        public AsteroidController CreateAsteroidInsideAsteroidCloud(Vector3 spawnPoint, SingleAsteroidConfig config, GameObject pool, Vector3 asteroidCloudSize)
        {
            var spawnRadius = asteroidCloudSize.MaxVector3CoordinateOnPlane() / 2;
            var spawnPosition = (Vector2)spawnPoint + Random.insideUnitCircle * spawnRadius;

            if (TryGetSpawnPointIncideAsteroidCloud(spawnPoint, config, spawnRadius, ref spawnPosition)) return CreateAsteroidInPool(spawnPosition, config, pool);
            else return null;
        }

        public AsteroidController CreateAsteroidInsideAsteroidCloudAfterCollision(
            Vector3 spawnPoint,
            SingleAsteroidConfig config,
            GameObject pool,
            Vector3 asteroidCloudSize,
            Collision2D collision)
        {
            var spawnRadius = asteroidCloudSize.MaxVector3CoordinateOnPlane() / 2;
            var spawnPosition = (Vector2)spawnPoint + Random.insideUnitCircle * spawnRadius;

            if (TryGetSpawnPointIncideAsteroidCloud(spawnPoint, config, spawnRadius, ref spawnPosition)) return CreateAsteroidAfterCollision(spawnPosition, config, pool, collision);
            else return null;
        }

        public List<AsteroidController> CreateAsteroidCloud(Vector3 spawnPosition, AsteroidCloudConfig config)
        {
            var asteroidCloudPool = CreateAsteroidCloudPool(config);

            var asteroidControllersOutput = new List<AsteroidController>();
            var asteroidsInCloud = Random.Range(config.MinAsteroidsInCloud, config.MaxAsteroidsInCloud + 1);

            CreateAsteroidsInCloud(spawnPosition, config, asteroidCloudPool, asteroidControllersOutput, asteroidsInCloud);

            return asteroidControllersOutput;
        }

        public List<AsteroidController> CreateAsteroidCloudAfterAsteroidDestroyed(AsteroidView creatorView, AsteroidCloudConfig config)
        {
            var asteroidCloudPool = CreateAsteroidCloudPool(config);

            var asteroidControllersOutput = new List<AsteroidController>();
            var asteroidsInCloud = Random.Range(config.MinAsteroidsInCloud, config.MaxAsteroidsInCloud + 1);

            SpawnAsteroidsFromDestroyedAsteroid(creatorView, config, asteroidCloudPool, asteroidControllersOutput, asteroidsInCloud);

            return asteroidControllersOutput;
        }

        public List<AsteroidController> CreateAsteroidCloudAfterCollision(Vector3 spawnPosition, AsteroidCloudConfig config, Collision2D collision)
        {
            var asteroidCloudPool = CreateAsteroidCloudPool(config);

            var asteroidControllersOutput = new List<AsteroidController>();
            var asteroidsInCloud = Random.Range(config.MinAsteroidsInCloud, config.MaxAsteroidsInCloud + 1);

            SpawnAsteroidsInCloudAfterCollision(spawnPosition, config, asteroidCloudPool, asteroidControllersOutput, asteroidsInCloud, collision);

            return asteroidControllersOutput;
        }

        public AsteroidController CreateAsteroidOnDistanceFromPlayer(SingleAsteroidConfig config) => CreateAsteroidOnRadius(_player.transform.position, config);

        private AsteroidView CreateAsteroidView(Vector3 spawnPosition, SingleAsteroidConfig config)
        {
            var asteroid = InstantiateAsteroid(spawnPosition, config);
            asteroid.transform.SetParent(_pool.transform, true);
            return asteroid;
        }

        private AsteroidView CreateAsteroidView(Vector3 spawnPosition, SingleAsteroidConfig config, GameObject asteroidCloudPool)
        {
            var asteroid = InstantiateAsteroid(spawnPosition, config);
            asteroid.transform.SetParent(asteroidCloudPool.transform, true);
            return asteroid;
        }

        private AsteroidView InstantiateAsteroid(Vector3 spawnPosition, SingleAsteroidConfig config)
        {
            var asteroid = Object.Instantiate(config.Prefab, spawnPosition, Quaternion.identity);
            asteroid.transform.name = config.Size.SizeType.ToString() + config.AsteroidType.ToString();
            var baseAsteroidScale = asteroid.transform.localScale;
            asteroid.transform.localScale = new Vector3(
                baseAsteroidScale.x * config.Size.AsteroidScale.x,
                baseAsteroidScale.y * config.Size.AsteroidScale.y,
                baseAsteroidScale.z * asteroid.transform.localScale.z);
            return asteroid;
        }

        #endregion


        #region SupportMethods

        private bool TryGetSpawnPointOnRadius(Vector3 spawnPoint, SingleAsteroidConfig config, ref Vector2 spawnPosition)
        {
            int spawnTries = 0;
            do
            {
                spawnPosition = UnityHelper.GetAPointOnRadius(spawnPoint, config.Behaviour.SpawnRadius);
                spawnTries++;
            }
            while (UnityHelper.IsAnyObjectAtPosition(spawnPosition, config.Size.AsteroidScale.MaxVector3CoordinateOnPlane()) && spawnTries <= max_spawn_tries);

            if (spawnTries > max_spawn_tries) return false;
            else return true;
        }

        private bool TryGetSpawnPointNearSpawnPoint(Vector3 spawnPoint, SingleAsteroidConfig config, ref Vector2 spawnPosition)
        {
            int spawnTries = 0;
            do
            {
                spawnPosition = (Vector2)spawnPoint + Random.insideUnitCircle * config.Behaviour.SpawnRadius;
                spawnTries++;
            }
            while (UnityHelper.IsAnyObjectAtPosition(spawnPosition, config.Size.AsteroidScale.MaxVector3CoordinateOnPlane()) && spawnTries <= max_spawn_tries);

            if (spawnTries > max_spawn_tries) return false;
            else return true;
        }

        private bool TryGetSpawnPointIncideAsteroidCloud(Vector3 spawnPoint, SingleAsteroidConfig config, float spawnRadius, ref Vector2 spawnPosition)
        {
            int spawnTries = 0;
            do
            {
                spawnPosition = (Vector2)spawnPoint + Random.insideUnitCircle * spawnRadius;
                spawnTries++;
            }
            while (UnityHelper.IsAnyObjectAtPosition(spawnPosition, config.Size.AsteroidScale.MaxVector3CoordinateOnPlane()) && spawnTries <= max_spawn_tries);

            if (spawnTries > max_spawn_tries) return false;
            else return true;
        }

        private GameObject CreateAsteroidCloudPool(AsteroidCloudConfig config)
        {
            var asteroidCloudPool = new GameObject(config.CloudType.ToString());
            asteroidCloudPool.transform.SetParent(_pool.transform, false);
            return asteroidCloudPool;
        }

        private void CreateAsteroidsInCloud(
            Vector3 spawnPosition,
            AsteroidCloudConfig config,
            GameObject asteroidCloudPool,
            List<AsteroidController> asteroidControllersOutput,
            int asteroidsInCloud)
        {
            int spawnTries = 0;
            while (asteroidControllersOutput.Count < asteroidsInCloud && spawnTries <= max_spawn_tries)
            {
                var random = new System.Random();
                var asteroidConfigToSpawn = RandomPicker.PickOneElementByWeights(config.CloudAsteroidsConfigs, random);

                var currentAsteroid = CreateAsteroidInsideAsteroidCloud(
                    spawnPosition,
                    asteroidConfigToSpawn,
                    asteroidCloudPool,
                    config.AsteroidCloudSize);
                if (currentAsteroid == null) spawnTries++;
                else asteroidControllersOutput.Add(currentAsteroid);
            }
        }

        private void SpawnAsteroidsInCloudAfterCollision(
            Vector3 spawnPosition,
            AsteroidCloudConfig config,
            GameObject asteroidCloudPool,
            List<AsteroidController> asteroidControllersOutput,
            int asteroidsInCloud,
            Collision2D collision)
        {
            int spawnTries = 0;
            while (asteroidControllersOutput.Count < asteroidsInCloud && spawnTries <= max_spawn_tries)
            {
                var random = new System.Random();
                var asteroidConfigToSpawn = RandomPicker.PickOneElementByWeights(config.CloudAsteroidsConfigs, random);


                var currentAsteroid = CreateAsteroidInsideAsteroidCloudAfterCollision(
                    spawnPosition,
                    asteroidConfigToSpawn,
                    asteroidCloudPool,
                    config.AsteroidCloudSize,
                    collision);
                if (currentAsteroid == null) spawnTries++;
                else asteroidControllersOutput.Add(currentAsteroid);
            }
        }

        private void SpawnAsteroidsFromDestroyedAsteroid(
            AsteroidView creatorView,
            AsteroidCloudConfig config,
            GameObject asteroidCloudPool,
            List<AsteroidController> asteroidControllersOutput,
            int asteroidsInCloud)
        {
            int spawnTries = 0;
            while (asteroidControllersOutput.Count < asteroidsInCloud && spawnTries <= max_spawn_tries)
            {
                var random = new System.Random();
                var asteroidConfigToSpawn = RandomPicker.PickOneElementByWeights(config.CloudAsteroidsConfigs, random);


                var currentAsteroid = CreateAsteroidInsideAsteroidCloudAfterDestruction(
                    creatorView,
                    asteroidConfigToSpawn,
                    asteroidCloudPool,
                    config.AsteroidCloudSize);
                if (currentAsteroid == null) spawnTries++;
                else asteroidControllersOutput.Add(currentAsteroid);
            }
        }

        #endregion


    }
}
