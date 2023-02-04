using Abstracts;
using UnityEngine;
using Utilities.ResourceManagement;

namespace Gameplay.Asteroid
{
    public class AsteroidExplosionController : BaseController
    {
        private readonly ResourcePath _path = new(Constants.Prefabs.AsteroidExplosion.Explosion);

        public AsteroidDestructionView AsteroidDestructionView { get; set; }
        private GameObject _explosion;

        public AsteroidExplosionController()
        {
            _explosion = ResourceLoader.LoadObject<GameObject>(_path);
            AsteroidDestructionView = CreateAsteroidDestructionView(new Vector3(0,0,0));          
        }

        private AsteroidDestructionView CreateAsteroidDestructionView(Vector3 spawnPosition)
        {
            var explosion = Object.Instantiate(_explosion);
            AsteroidDestructionView explosionView = explosion?.GetComponent<AsteroidDestructionView>();
            return explosionView;
        }
    }
}