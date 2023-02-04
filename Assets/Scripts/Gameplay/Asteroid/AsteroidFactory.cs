using Scriptables.Asteroid;
using UnityEngine;


namespace Gameplay.Asteroid
{
    public class AsteroidFactory 
    {
        private readonly AsteroidConfig _config;
        private readonly GameObject _pool;

        public AsteroidFactory(AsteroidConfig config, GameObject pool)
        {
            _config = config;
            _pool = pool;
        }

        public AsteroidController CreateAsteroid(Vector3 spawnPosition, GameObject pool) => new(_config, CreateAsteroidView(spawnPosition, pool));

        private AsteroidView CreateAsteroidView(Vector3 spawnPosition, GameObject pool)
        {
            var asteroid = Object.Instantiate(_config.Prefab, spawnPosition, Quaternion.identity);
            asteroid.transform.SetParent(pool.transform);
            Vector3 size = asteroid.transform.localScale;
            asteroid.transform.localScale = new Vector3(size.x + _config.AsteroidSize.x, size.y + _config.AsteroidSize.y, size.z);
            return asteroid;
        }
    }
}
