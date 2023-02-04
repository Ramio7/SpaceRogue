using Scriptables.Asteroid;
using UnityEngine;


namespace Gameplay.Asteroid
{
    public class AsteroidFactory 
    {
        private readonly AsteroidConfig _config;

        public AsteroidFactory(AsteroidConfig config)
        {
            _config = config;
        }

        public AsteroidController CreateAsteroid(Vector3 spawnPosition) => new(_config, CreateAsteroidView(spawnPosition));

        private AsteroidView CreateAsteroidView(Vector3 spawnPosition)
        {
            var asteroid = Object.Instantiate(_config.Prefab, spawnPosition, Quaternion.identity);
            Vector3 size = asteroid.transform.localScale;
            asteroid.transform.localScale = new Vector3(size.x + _config.AsteroidSize.x, size.y + _config.AsteroidSize.y, size.z);
            return asteroid;
        }
    }
}
