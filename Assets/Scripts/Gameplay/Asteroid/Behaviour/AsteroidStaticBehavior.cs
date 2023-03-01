using UnityEngine;
using Random = System.Random;

namespace Gameplay.Asteroid.Behaviour
{
    public class AsteroidStaticBehavior : AsteroidBehaviour
    {
        private Transform _asteroidTransform;
        private Vector3 _rotationVector;
        private float _rotationSpeed;

        public AsteroidStaticBehavior(AsteroidView view, AsteroidBehaviourConfig config) : base(view, config)
        {
            _asteroidTransform = view.GetComponent<Transform>();
            Random random = new();
            _rotationSpeed = random.Next(1,10);
            _rotationVector = (random.Next(2) == 1) ? new Vector3(0, 0, -_rotationSpeed) : new Vector3(0, 0, _rotationSpeed);
        }
        
        protected override void OnUpdate()
        {
            AsteroidRotation();
        }

        private void AsteroidRotation()
        {
            _asteroidTransform.Rotate(_rotationVector);
        }   
    }
}