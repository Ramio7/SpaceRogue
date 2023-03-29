using System.Collections.Generic;
using System.Linq;
using Gameplay.Damage;
using Gameplay.Mechanics.Timer;
using Gameplay.Survival;
using UnityEngine;

namespace Gameplay.Space.SpaceObjects.SpaceObjectsEffects.Views
{
    public sealed class DamageAuraView : AreaEffectView, IDamagingView
    {
        public DamageModel DamageModel { get; private set; }

        private Timer _damageTimer;
        private List<IDamageableView> _damageablesInAura;

        public void Init(DamageModel damageModel, Timer damageTimer)
        {
            _damageablesInAura = new List<IDamageableView>();
            DamageModel = damageModel;
            
            _damageTimer = damageTimer;
            _damageTimer.OnExpire += DamageTick;
            
            _damageTimer.Start();
        }

        private void OnDestroy()
        {
            _damageTimer.OnExpire -= DamageTick;
            _damageablesInAura.Clear();
            _damageTimer.Dispose();
        }

        public void DealDamage(IDamageableView damageable)
        {
            damageable.TakeDamage(DamageModel);
        }
        
        public void OnTriggerEnter2D(Collider2D other)
        {   
            CollisionEnter(other.gameObject);
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            CollisionEnter(collision.gameObject);
        }

        private void CollisionEnter(GameObject go)
        {
            var damageable = go.GetComponent<IDamageableView>();
            if (damageable is not null)
            {
                _damageablesInAura.Add(damageable);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {   
            CollisionExit(other.gameObject);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            CollisionExit(collision.gameObject);
        }

        private void CollisionExit(GameObject go)
        {
            var damageable = go.GetComponent<IDamageableView>();
            if (damageable is not null)
            {
                _damageablesInAura.Remove(damageable);
            }
        }

        private void DamageTick()
        {
            if (!_damageablesInAura.Any())
            {
                _damageTimer.Start();
                return;
            }

            foreach (var damageable in _damageablesInAura)
            {
                damageable?.TakeDamage(DamageModel);
            }
            
            _damageTimer.Start();
        }
    }
}