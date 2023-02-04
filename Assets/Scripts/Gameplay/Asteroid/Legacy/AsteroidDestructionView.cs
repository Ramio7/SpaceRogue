using UnityEngine;
namespace Gameplay.Asteroid
{
    public class AsteroidDestructionView : MonoBehaviour
    {
        [SerializeField] private GameObject _explosion;
        [SerializeField] private ParticleSystem _particle1;
        [SerializeField] private ParticleSystem _particle2;
        private float _currentAsteroidSpawnTime = 0.7f;

        public void PlayExplosion(Vector3 position)
        {
            _explosion.transform.position = position;
            _particle1?.Play();
            _particle2?.Play();
        }

        protected void OnUpdate()
        {
            if (_currentAsteroidSpawnTime <= 0.0f)
            {
                Destroy(gameObject);
            }
            CooldownAsteroidSpawn();
        }

        protected void CooldownAsteroidSpawn()
        {
            _currentAsteroidSpawnTime -= Time.deltaTime;
        }
    }
}