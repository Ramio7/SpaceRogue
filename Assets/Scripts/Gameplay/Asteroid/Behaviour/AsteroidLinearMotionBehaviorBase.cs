using Gameplay.Health;
using UnityEngine;

namespace Gameplay.Asteroid.Behaviour
{
    public abstract class AsteroidLinearMotionBehaviorBase : AsteroidBehaviour
    {
        protected Rigidbody2D Rigidbody;
        protected Vector3 AsteroidDirection;
        private readonly HealthController _healthController;

        public AsteroidLinearMotionBehaviorBase(AsteroidView view, AsteroidBehaviourConfig config, HealthController healthController) : base(view, config)
        {
            Rigidbody = View.GetComponent<Rigidbody2D>();
            
            if (Config.AsteroidLifeTime != 0)
            {
                Timer = new(Config.AsteroidLifeTime);
                Timer.Start();
                Timer.OnExpire += DestroyAsteroidOnTimerExpired;
            }

            _healthController = healthController;
        }

        protected abstract void AsteroidStart();

        protected void DestroyAsteroidOnTimerExpired()
        {
            _healthController.DestroyUnit();
            Timer.Dispose();
            OnDispose();
        }

        protected override void OnDispose()
        {
            Rigidbody = null;
        }

        protected void Move(Vector2 direction, float startingForce)
        {
            Rigidbody.AddForce(startingForce * direction, ForceMode2D.Force);
        }
    }
}
