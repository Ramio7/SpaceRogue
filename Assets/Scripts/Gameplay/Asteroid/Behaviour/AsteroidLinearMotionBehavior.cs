using UnityEngine;
using Utilities.Mathematics;

namespace Gameplay.Asteroid.Behaviour
{
    public class AsteroidLinearMotionBehavior : AsteroidBehaviour
    {
        protected Rigidbody2D Rigidbody;
        protected Vector3 AsteroidDirection;

        public AsteroidLinearMotionBehavior(AsteroidView view, AsteroidBehaviourConfig config) : base(view, config)
        {
            Rigidbody = View.GetComponent<Rigidbody2D>();

            if (Config.AsteroidLifeTime != 0)
            {
                Timer = new(Config.AsteroidLifeTime);
                Timer.Start();
                Timer.OnExpire += DestroyAsteroidOnTimerExpired;
            }

            AsteroidStart();
        }

        protected virtual void AsteroidStart()
        {
            AsteroidDirection = RandomPicker.PickRandomAngle(0, 360, new());
            Move(AsteroidDirection, Config.AsteroidStartingForce);
        }

        protected void DestroyAsteroidOnTimerExpired()
        {
            if (Config.AsteroidLifeTime != 0)
            {
                Object.Destroy(View.gameObject);
                Timer.Dispose();
                Dispose();
            }
        }

        protected override void OnDispose()
        {
            Dispose();
            Rigidbody = null;
        }

        protected void Move(Vector2 direction, float startingForce)
        {
            Rigidbody.AddForce(startingForce * Time.deltaTime * direction, ForceMode2D.Force);
        }
    }
}
