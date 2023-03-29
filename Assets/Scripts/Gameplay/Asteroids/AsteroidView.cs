using Abstracts;
using Gameplay.Abstracts;
using Gameplay.Damage;
using Gameplay.Survival;
using UnityEngine;

namespace Gameplay.Asteroids
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
    public class AsteroidView : EntityView, IDamagingView
    {
        public DamageModel DamageModel { get; private set; }

        public override EntityType EntityType => EntityType.Asteroid;

        public void Init(float damage)
        {
            DamageModel = new(damage, EntityType);
        }

        public void DealDamage(IDamageableView damageable)
        {
            damageable.TakeDamage(DamageModel);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamageableView victim)) DealDamage(victim); 
            //TODO asteroid destroy on collision with heavy objects
        }
    }
}