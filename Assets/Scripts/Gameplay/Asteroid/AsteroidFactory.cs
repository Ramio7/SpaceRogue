using Asteroid;
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

        public AsteroidController CreateAsteroid(Vector3 spawnPosition, SingleAsteroidConfig config, GameObject pool) =>
            new(config, CreateAsteroidView(spawnPosition, config, pool), _player);

        public AsteroidController CreateAsteroid(Vector3 spawnPosition, SingleAsteroidConfig config, GameObject pool, Collision2D collision) =>
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
            if (TryGetSpawnPoint(spawnPoint, config, ref spawnPosition)) return CreateAsteroid(spawnPosition, config, _pool);
            else return null;
        }

        public AsteroidController CreateAsteroidInsideRadius(Vector3 spawnPoint, SingleAsteroidConfig config, GameObject pool, Vector3 asteroidCloudSize)
        {
            var spawnRadius = asteroidCloudSize.MaxVector3CoordinateOnPlane() / 2;
            var spawnPosition = (Vector2)spawnPoint + Random.insideUnitCircle * spawnRadius;

            if (TryGetSpawnPoint(spawnPoint, config, spawnRadius, ref spawnPosition)) return CreateAsteroid(spawnPosition, config, pool);
            else return null;
        }

        public AsteroidController CreateAsteroidInsideRadius(Vector3 spawnPoint, SingleAsteroidConfig config, GameObject pool, Vector3 asteroidCloudSize, Collision2D collision)
        {
            var spawnRadius = asteroidCloudSize.MaxVector3CoordinateOnPlane() / 2;
            var spawnPosition = (Vector2)spawnPoint + Random.insideUnitCircle * spawnRadius;

            if (TryGetSpawnPoint(spawnPoint, config, spawnRadius, ref spawnPosition)) return CreateAsteroid(spawnPosition, config, pool, collision);
            else return null;
        }

        public List<AsteroidController> CreateAsteroidCloud(Vector3 spawnPosition, AsteroidCloudConfig config)
        {
            var asteroidCloudPool = CreateAsteroidCloudPool(config);

            var asteroidControllersOutput = new List<AsteroidController>();
            var asteroidsInCloud = Random.Range(config.MinAsteroidsInCloud, config.MaxAsteroidsInCloud + 1);

            SpawnAsteroids(spawnPosition, config, asteroidCloudPool, asteroidControllersOutput, asteroidsInCloud);

            return asteroidControllersOutput;
        }

        public List<AsteroidController> CreateAsteroidCloud(AsteroidView asteroidView, AsteroidCloudConfig config)
        {
            var asteroidCloudPool = CreateAsteroidCloudPool(config);

            var asteroidControllersOutput = new List<AsteroidController>();
            var asteroidsInCloud = Random.Range(config.MinAsteroidsInCloud, config.MaxAsteroidsInCloud + 1);
            SetAsteroidInCloudMoveType(config);

            SpawnAsteroids(asteroidView, config, asteroidCloudPool, asteroidControllersOutput, asteroidsInCloud);

            return asteroidControllersOutput;
        }

        public List<AsteroidController> CreateAsteroidCloud(Vector3 spawnPosition, AsteroidCloudConfig config, Collision2D collision)
        {
            var asteroidCloudPool = CreateAsteroidCloudPool(config);

            var asteroidControllersOutput = new List<AsteroidController>();
            var asteroidsInCloud = Random.Range(config.MinAsteroidsInCloud, config.MaxAsteroidsInCloud + 1);

            SpawnAsteroids(spawnPosition, config, asteroidCloudPool, asteroidControllersOutput, asteroidsInCloud, collision);

            return asteroidControllersOutput;
        }

        public AsteroidController CreateAsteroidNearPlayer(SingleAsteroidConfig config) => CreateAsteroidOnRadius(_player.transform.position, config);

        private AsteroidView CreateAsteroidView(Vector3 spawnPosition, SingleAsteroidConfig config)
        {
            var asteroid = Instantiate(spawnPosition, config);
            asteroid.transform.SetParent(_pool.transform, true);
            return asteroid;
        }

        private AsteroidView CreateAsteroidView(Vector3 spawnPosition, SingleAsteroidConfig config, GameObject asteroidCloudPool)
        {
            var asteroid = Instantiate(spawnPosition, config);
            asteroid.transform.SetParent(asteroidCloudPool.transform, true);
            return asteroid;
        }

        private AsteroidView Instantiate(Vector3 spawnPosition, SingleAsteroidConfig config)
        {
            var asteroid = Object.Instantiate(config.Prefab, spawnPosition, Quaternion.identity);
            asteroid.transform.name = config.Size.SizeType.ToString() + config.AsteroidType.ToString();
            asteroid.transform.localScale = new Vector3(config.Size.AsteroidScale.x, config.Size.AsteroidScale.y, asteroid.transform.localScale.z);
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

        private bool TryGetSpawnPoint(Vector3 spawnPoint, SingleAsteroidConfig config, ref Vector2 spawnPosition)
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

        private bool TryGetSpawnPoint(Vector3 spawnPoint, SingleAsteroidConfig config, float spawnRadius, ref Vector2 spawnPosition)
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

        private void SpawnAsteroids(
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

                var currentAsteroid = CreateAsteroidInsideRadius(
                    spawnPosition,
                    asteroidConfigToSpawn,
                    asteroidCloudPool,
                    config.AsteroidCloudSize);
                if (currentAsteroid == null) spawnTries++;
                else asteroidControllersOutput.Add(currentAsteroid);
            }
        }

        private void SpawnAsteroids(Vector3 spawnPosition,
                                    AsteroidCloudConfig config,
                                    GameObject asteroidCloudPool,
                                    List<AsteroidController> asteroidControllersOutput,
                                    int asteroidsInCloud,
                                    Collision2D collision)
        {
            int spawnTries = 0;
            while (asteroidControllersOutput.Count < asteroidsInCloud && spawnTries <= _maxSpawnTries)
            {
                var random = new System.Random();
                var currentConfig = Random.Range(0, config.CloudAsteroidsConfigs.Count);

                if (RandomPicker.TakeChance(config.CloudAsteroidsConfigs[currentConfig].SpawnChance, random))
                {
                    var currentAsteroid = CreateAsteroidInsideRadius(spawnPosition,
                                                                     config.CloudAsteroidsConfigs[currentConfig],
                                                                     asteroidCloudPool,
                                                                     config.AsteroidCloudSize,
                                                                     collision);
                    if (currentAsteroid == null) spawnTries++;
                    if (currentAsteroid != null) asteroidControllersOutput.Add(currentAsteroid);
                }
            }
        }

        private void SpawnAsteroids(
            AsteroidView asteroidView,
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


                var currentAsteroid = CreateAsteroidInsideRadius(
                    asteroidView.transform.position,
                    asteroidConfigToSpawn,
                    asteroidCloudPool,
                    config.AsteroidCloudSize);
                if (currentAsteroid == null) spawnTries++;
                else asteroidControllersOutput.Add(currentAsteroid);
            }
        }


        private void SetAsteroidInCloudMoveType(AsteroidCloudConfig config)
        {
            switch (config.Behavior)
            {
                case AsteroidCloudBehaviour.None:
                    {
                        throw new System.Exception("Cloud behaviour not set");
                    }
                case AsteroidCloudBehaviour.Static:
                    {
                        SetAsteroidMoveType(config, AsteroidMoveType.Static);
                        break;
                    }
                case AsteroidCloudBehaviour.CreatorEscaping:
                    {
                        SetAsteroidMoveType(config, AsteroidMoveType.CreatorEscaping);
                        break;
                    }
                case AsteroidCloudBehaviour.CollisionEscaping:
                    {
                        SetAsteroidMoveType(config, AsteroidMoveType.CollisionEscaping);
                        break;
                    }
                default: throw new System.Exception("No such cloud behavior typr in method");
            }
        }

        private void SetAsteroidMoveType(AsteroidCloudConfig config, AsteroidMoveType asteroidMoveType)
        {
            for (int i = 0; i < config.CloudAsteroidsConfigs.Count; i++)
            {
                config.CloudAsteroidsConfigs[i].Config.Behaviour.AsteroidMoveType = asteroidMoveType;
            }
        }

        #endregion


    }
}
