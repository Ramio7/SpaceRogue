using Gameplay.Player;
using Scriptables.Asteroid;
using System.Collections.Generic;
using UnityEngine;


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

        private AsteroidView CreateAsteroidView(Vector3 spawnPosition, AsteroidConfig config)
        {
            var asteroid = Object.Instantiate(config.Prefab, spawnPosition, Quaternion.identity);
            asteroid.transform.name = config.AsteroidType.ToString();
            asteroid.transform.SetParent(_pool.transform, true);
            Vector3 size = asteroid.transform.localScale;
            asteroid.transform.localScale = new Vector3(size.x + config.AsteroidSize.x, size.y + config.AsteroidSize.y, size.z);
            return asteroid;
        }

        public void Dispose()
        {
            _pool = null;
            _player = null;
        }
    }
}
