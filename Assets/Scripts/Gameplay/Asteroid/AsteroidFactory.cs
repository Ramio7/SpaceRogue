using Gameplay.Player;
using Scriptables.Asteroid;
using UnityEngine;
using Utilities.Mathematics;
using Utilities.Unity;

namespace Gameplay.Asteroid
{
    public class AsteroidFactory 
    {
        private GameObject _pool;
        private PlayerView _player;

        public AsteroidFactory(GameObject pool, PlayerView player)
        {
            _pool = pool;
            _player = player;
        }

        public AsteroidController CreateAsteroid(Vector3 spawnPosition, AsteroidConfig config) => new(config, CreateAsteroidView(spawnPosition, config), _player);

        public AsteroidController CreateAsteroidOnRadius(Vector3 spawnPoint, AsteroidConfig config)
        {
            var spawnPosition = UnityHelper.GetAPointOnRadius(spawnPoint, config.Behaviour.SpawnRadius);

            while (UnityHelper.IsAnyObjectAtPosition(spawnPosition, config.Size.AsteroidScale.MaxVector3CoordinateOnPlane())) 
            {
                spawnPosition = UnityHelper.GetAPointOnRadius(spawnPoint, config.Behaviour.SpawnRadius);
            }

            return CreateAsteroid(spawnPosition, config);
        }

        public AsteroidController CreateAsteroidNearPlayer(AsteroidConfig config) => CreateAsteroidOnRadius(_player.transform.position, config);

        private AsteroidView CreateAsteroidView(Vector3 spawnPosition, AsteroidConfig config)
        {
            var asteroid = Object.Instantiate(config.Prefab, spawnPosition, Quaternion.identity);
            asteroid.transform.name = config.AsteroidType.ToString();
            asteroid.transform.SetParent(_pool.transform, true);
            Vector3 size = asteroid.transform.localScale;
            asteroid.transform.localScale = new Vector3(size.x + config.Size.AsteroidScale.x, size.y + config.Size.AsteroidScale.y, size.z);
            return asteroid;
        }

        public void Dispose()
        {
            _pool = null;
            _player = null;
        }
    }
}
