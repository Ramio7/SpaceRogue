using System;
using Gameplay.Damage;
using Gameplay.Survival;
using UnityEngine;

namespace Gameplay.Abstracts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class EntityView : MonoBehaviour, IDamageableView
    {
        public abstract EntityType EntityType { get; }
        public event Action<DamageModel> DamageTaken = _ => { };

        public void TakeDamage(DamageModel damageModel)
        {
            DamageTaken(damageModel);
        }
    }
}