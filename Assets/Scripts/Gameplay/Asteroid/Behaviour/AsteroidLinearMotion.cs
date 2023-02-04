using UnityEngine;
using Utilities.Mathematics;

namespace Gameplay.Asteroid.Behaviour
{
    public class AsteroidLinearMotion : AsteroidBehaviour
    {
        private Rigidbody2D _rigidbody;
        private Vector2 _asteroidDirection;
        private readonly float _speed;

        public AsteroidLinearMotion(AsteroidView view, AsteroidBehaviourConfig config) : base(view, config)
        {
            _speed = config.AsteroidSpeed;
            _asteroidDirection = RandomPicker.PickRandomAngle(0, 360, new());

            _rigidbody = _view.GetComponentInParent<Rigidbody2D>();
        }

        protected override void OnUpdate()
        {
            Move(_asteroidDirection, _speed);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            _rigidbody = null;
        }

        private void Move(Vector2 direction, float speed)
        {
            if (_rigidbody.velocity.normalized.sqrMagnitude <= speed)
            _rigidbody.AddForce(direction * speed * Time.deltaTime, ForceMode2D.Force);
        }
    }
}
