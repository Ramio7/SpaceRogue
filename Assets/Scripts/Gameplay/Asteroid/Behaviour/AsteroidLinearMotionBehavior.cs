using UnityEngine;
using Utilities.Mathematics;

namespace Gameplay.Asteroid.Behaviour
{
    public class AsteroidLinearMotionBehavior : AsteroidBehaviour
    {
        protected Rigidbody2D _rigidbody;

        public AsteroidLinearMotionBehavior(AsteroidView view, AsteroidBehaviourConfig config) : base(view, config)
        {
            var asteroidDirection = RandomPicker.PickRandomAngle(0, 360, new());
            _rigidbody = _view.GetComponent<Rigidbody2D>();
            Move(asteroidDirection, config.AsteroidStartingForce);
        }

        protected override void OnDispose()
        {
            Dispose();
            _rigidbody = null;
        }

        protected void Move(Vector2 direction, float startingForce)
        {
            _rigidbody.AddForce(startingForce * Time.deltaTime * direction, ForceMode2D.Force);
        }
    }
}
