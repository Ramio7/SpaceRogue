using Abstracts;
using Gameplay.Abstracts;
using Gameplay.Damage;
using Gameplay.Space.SpaceObjects;
using UnityEngine;

namespace Gameplay.Enemy
{
    public sealed class EnemyView : EntityView
    {
        public override EntityType EntityType => EntityType.Enemy;

        public void OnTriggerEnter2D(Collider2D collider)
        {
            CollisionEnter(collider.gameObject);
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            CollisionEnter(collision.gameObject);
        }

        private void CollisionEnter(GameObject go)
        {
            if (go.TryGetComponent(out IDamagingView view) && !go.TryGetComponent(out SpaceObjectView _))
            {
                TakeDamage(view.DamageModel);
            }
        }
    }
}