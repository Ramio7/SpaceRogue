using UnityEngine;
using Random = System.Random;

namespace Gameplay.Asteroid.Behaviour
{
    public class AsteroidStaticBehavior : AsteroidBehaviour
    {
        private Transform _asteroidTransform;
        private Vector3 _rotationVector;

        private Random _random = new();
        private readonly float _rotationSpeed;

        public AsteroidStaticBehavior(AsteroidView view, AsteroidBehaviourConfig config) : base(view, config)
        {
            _asteroidTransform = view.GetComponent<Transform>();

            _rotationSpeed = (float)(_random.NextDouble() * config.MaxRotationSpeed);

            SetRotationDirection();
        }

        private void SetRotationDirection()
        {
            bool isRotatingClockwise = _random.Next(2) == 0;
            _rotationVector = isRotatingClockwise ? new Vector3(0, 0, -_rotationSpeed) : new Vector3(0, 0, _rotationSpeed);
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