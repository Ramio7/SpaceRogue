using Gameplay.Damage;
using Gameplay.Health;
using System;
using UnityEngine;


namespace Gameplay.Asteroid
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class AsteroidView : MonoBehaviour, IDamagingView, IDamageableView
    {
        public DamageModel DamageModel { get; private set; }

        public event Action<DamageModel> DamageTaken = (DamageModel _) => { };
        public event Action<Collision2D> CollisionEntered;
        public event Action AsteroidDestroyed;

        public void Init(DamageModel damageModel)
        {
            DamageModel = damageModel;
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamageableView victimView))
            {
                victimView.TakeDamage(this);
            }
            CollisionEntered?.Invoke(collision);
            AsteroidDestroyed?.Invoke();
        }

        public void TakeDamage(IDamagingView damageComponent)
        {
            DamageTaken(damageComponent.DamageModel);
        }
    }
}