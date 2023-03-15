using System;
using Gameplay.Damage;
using Gameplay.Survival;
using UnityEngine;

namespace Abstracts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class UnitView : MonoBehaviour, IDamageableView
    {
        [field: SerializeField] public UnitType UnitType { get; protected set; }
        
        public event Action<DamageModel> DamageTaken = _ => { };

        public void OnTriggerEnter2D(Collider2D other)
        {   
            CollisionEnter(other.gameObject);
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            CollisionEnter(collision.gameObject);
        }

        public void TakeDamage(IDamagingView damageComponent)
        {
            DamageTaken(damageComponent.DamageModel);
        }

        public void TakeDamage(DamageModel damageComponent)
        {
            DamageTaken(damageComponent);
        }

        private void CollisionEnter(GameObject go)
        {
            var damageComponent = go.GetComponent<IDamagingView>();
            if (damageComponent is not null)
            {
                TakeDamage(damageComponent);
            }
        }
    }
}