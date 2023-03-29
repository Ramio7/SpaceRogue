using System;
using Gameplay.Abstracts;
using Gameplay.Damage;
using Gameplay.Survival.DamageImmunityFrame;
using Gameplay.Survival.Health;
using Gameplay.Survival.Shield;
using Utilities.Mathematics;

namespace Gameplay.Survival
{
    public sealed class EntitySurvival : IDisposable
    {
        private readonly EntityView _entityView;
        private readonly EntityDamageImmunityFrame _entityDamageImmunityFrame;

        public EntityHealth EntityHealth { get; }
        public EntityShield EntityShield { get; }
        public event Action UnitDestroyed = () => { };

        public EntitySurvival(EntityView entityView, EntityHealth entityHealth, EntityShield entityShield, EntityDamageImmunityFrame entityDamageImmunityFrame)
        {
            EntityHealth = entityHealth;
            EntityShield = entityShield;
            _entityView = entityView;
            _entityDamageImmunityFrame = entityDamageImmunityFrame;

            EntityHealth.HealthReachedZero += OnHealthReachedZero;
            _entityView.DamageTaken += ReceiveDamage;
        }

        public void Dispose()
        {
            _entityView.DamageTaken -= ReceiveDamage;
            EntityHealth.HealthReachedZero -= OnHealthReachedZero;
            
            EntityHealth.Dispose();
            EntityShield?.Dispose();
            _entityDamageImmunityFrame?.Dispose();
        }

        internal void ReceiveDamage(DamageModel damageModel)
        {
            var damage = RandomPicker.PickRandomBetweenTwoValues(damageModel.MinDamage, damageModel.MaxDamage);
            TakeDamage(damage);
        }

        internal void TakeDamage(float damageAmount)
        {
            if (_entityDamageImmunityFrame is not null && _entityDamageImmunityFrame.TryBlockDamage()) return;
            if (EntityShield is null || EntityShield.CurrentShield == 0.0f)
            {
                TakeFullDamageToHealth(damageAmount);
            }
            else
            {
                TakeDamageToShieldThenHealth(damageAmount);
            }
        }

        internal void RestoreHealth(float healthAmount)
        {
            EntityHealth.Heal(healthAmount);
        }

        private void TakeFullDamageToHealth(float damageAmount)
        {
            EntityHealth.TakeDamage(damageAmount);
        }
        
        private void TakeDamageToShieldThenHealth(float damageAmount)
        {
            EntityShield.TakeDamage(damageAmount, out float remainingDamage);
            if (remainingDamage > 0.0f) EntityHealth.TakeDamage(remainingDamage);
        }

        private void OnHealthReachedZero()
        {
            UnitDestroyed.Invoke();
        }
    }
}