using System;
using Abstracts;
using Gameplay.Damage;
using Scriptables.Health;
using UI.Game;

namespace Gameplay.Health
{
    public sealed class HealthController : BaseController
    {
        private const int FatalDamage = 9999;

        private readonly HealthStatusBarView _statusBarView;
        private readonly BaseHealthModel _healthModel;
        private readonly IDamageableView _damageable;
        
        private Action _onDestroy;

        public HealthStatusBarView StatusBarView => _statusBarView;

        public HealthController(HealthConfig healthConfig, ShieldConfig shieldConfig, HealthShieldStatusBarView statusBarView, IDamageableView damageable, float health = 0, float shield = 0)
        {
            var healthModel = new HealthWithShieldModel(healthConfig, shieldConfig, health, shield);
            
            statusBarView.HealthBar.Init(0.0f, healthModel.MaximumHealth.Value, healthModel.CurrentHealth.Value);
            statusBarView.ShieldBar.Init(0.0f, healthModel.MaximumShield.Value, healthModel.CurrentShield.Value);
            
            healthModel.CurrentHealth.Subscribe(statusBarView.HealthBar.UpdateValue);
            healthModel.CurrentShield.Subscribe(statusBarView.ShieldBar.UpdateValue);
            EntryPoint.SubscribeToUpdate(healthModel.UpdateState);

            damageable.DamageTaken += TakeDamage;
            _damageable = damageable;

            _statusBarView = statusBarView;
            _healthModel = healthModel;
        }
        
        public HealthController(HealthConfig healthConfig, ShieldConfig shieldConfig, IDamageableView damageable, float health = 0, float shield = 0)
        {
            var healthModel = new HealthWithShieldModel(healthConfig, shieldConfig, health, shield);
            
            EntryPoint.SubscribeToUpdate(healthModel.UpdateState);
            
            damageable.DamageTaken += TakeDamage;
            _damageable = damageable;
            
            _healthModel = healthModel;
        }

        public HealthController(HealthConfig healthConfig, HealthStatusBarView statusBarView, IDamageableView damageable, float health = 0)
        {
            var healthModel = new HealthOnlyModel(healthConfig, health);
            statusBarView.HealthBar.Init(0.0f, healthModel.MaximumHealth.Value, healthModel.CurrentHealth.Value);
            _statusBarView = statusBarView;

            damageable.DamageTaken += TakeDamage;
            _damageable = damageable;
            
            healthModel.CurrentHealth.Subscribe(statusBarView.HealthBar.UpdateValue);
            _healthModel = healthModel;
        }
        
        public HealthController(HealthConfig healthConfig, IDamageableView damageable, float health = 0)
        {
            var healthModel = new HealthOnlyModel(healthConfig, health);
            
            damageable.DamageTaken += TakeDamage;
            _damageable = damageable;
            
            EntryPoint.SubscribeToUpdate(healthModel.UpdateState);
            _healthModel = healthModel;
        }

        public void SubscribeToOnDestroy(Action onDestroyAction)
        {
            _onDestroy += onDestroyAction;
            _healthModel.UnitDestroyed += onDestroyAction;
        }

        protected override void OnDispose()
        {
            _damageable.DamageTaken -= TakeDamage;
            _healthModel.UnitDestroyed -= _onDestroy;
            EntryPoint.UnsubscribeFromUpdate(_healthModel.UpdateState);
            
            if (_statusBarView is not null) 
                _healthModel.CurrentHealth.Unsubscribe(_statusBarView.HealthBar.UpdateValue);

            if (_healthModel is HealthWithShieldModel healthShieldModel && _statusBarView is HealthShieldStatusBarView statusShieldBar) 
                healthShieldModel.CurrentShield.Unsubscribe(statusShieldBar.ShieldBar.UpdateValue);

        }

        public float GetCurrentHealth()
        {
            if (_statusBarView is not null)
            {
                return _healthModel.CurrentHealth.Value; 
            }
            return 0;
        }

        public float GetCurrentShield()
        {
            if (_healthModel is HealthWithShieldModel healthShieldModel && _statusBarView is HealthShieldStatusBarView)
            {
                return healthShieldModel.CurrentShield.Value; 
            }
            return 0;
        }

        public void DestroyUnit()
        {
            _healthModel.TakeDamage(FatalDamage);
        }

        private void TakeDamage(DamageModel damageModel)
        {
            _healthModel.TakeDamage(damageModel.DamageAmount);
        }
    }
}