using UnityEngine;
using Random = System.Random;

namespace Gameplay.Asteroid.Behaviour
{
    public class AsteroidStaticBehavior : AsteroidBehaviour
    {
        Transform _asteroidTransform;
        Vector3 _vector;

        private float _rotationSpeed = 0.1f;

        public AsteroidStaticBehavior(AsteroidView view,
                AsteroidBehaviourConfig config) : base(view, config)
        {
            _asteroidTransform = view.GetComponent<Transform>();
            Random random = new Random();
            _rotationSpeed = random.Next(1,10);
            _vector = (random.Next(2) == 1) ? new Vector3(0, 0, -_rotationSpeed) : new Vector3(0, 0, _rotationSpeed);
        }
        
        protected override void OnUpdate()
        {
            AsteroidRotation();
        }

        private void AsteroidRotation()
        {
            _asteroidTransform.Rotate(_vector);
        }   
    }
}