using System;
using Abstracts;
using Gameplay.Abstracts;
using Gameplay.Mechanics.Meter;
using Gameplay.Mechanics.Timer;
using Gameplay.Shooting.Factories;
using Gameplay.Shooting.Scriptables;
using UnityEngine;
using Utilities.Mathematics;

namespace Gameplay.Shooting.Weapons
{
    public class Minigun : Weapon, IDisposable
    {
        private readonly MinigunConfig _config;
        private readonly EntityType _entityType;
        private readonly ProjectileFactory _projectileFactory;
        private readonly MeterWithCooldown _overheatMeter;
        
        private float _currentSprayAngle;
        
        private float SprayIncrease => (_config.MaxSprayAngle - _config.SprayAngle) / (_config.TimeToOverheat * (1 / _config.Cooldown));

        public Minigun(MinigunConfig config, EntityType entityType, ProjectileFactory projectileFactory, TimerFactory timerFactory)
        {
            _config = config;
            _entityType = entityType;
            _projectileFactory = projectileFactory;
            CooldownTimer = timerFactory.Create(config.Cooldown);
            
            _overheatMeter = new MeterWithCooldown(0.0f, config.TimeToOverheat, config.OverheatCoolDown, timerFactory);
            _overheatMeter.OnCooldownEnd += ResetSpray;
            _currentSprayAngle = config.SprayAngle;
        }

        public new void Dispose()
        {
            _overheatMeter.OnCooldownEnd -= ResetSpray;
            _overheatMeter.Dispose();
            base.Dispose();
        }
        
        public override void CommenceFiring(Vector2 bulletPosition, Quaternion turretDirection)
        {
            if (_overheatMeter.IsOnCooldown || IsOnCooldown) return;

            FireSingleProjectile(bulletPosition, turretDirection);
            AddHeat();

            CooldownTimer.Start();
        }

        private void AddHeat()
        {
            _overheatMeter.Fill(_config.Cooldown);
            IncreaseSpray();
        }

        private void IncreaseSpray()
        {
            if (_currentSprayAngle >= _config.MaxSprayAngle) return;
            _currentSprayAngle += SprayIncrease;
        }

        private void ResetSpray()
        {
            _currentSprayAngle = _config.SprayAngle;
        }

        private void FireSingleProjectile(Vector2 bulletPosition, Quaternion turretDirection)
        {
            float angle = _currentSprayAngle / 2;

            float pelletAngle = RandomPicker.PickRandomBetweenTwoValues(-angle, angle);
            Vector3 pelletVector = (pelletAngle + 90).ToVector3();
            Quaternion pelletDirection = turretDirection * Quaternion.Euler(pelletVector.x, pelletVector.y, pelletVector.z);
            //TODO check 90 degrees turn
            _projectileFactory.Create(new ProjectileSpawnParams(bulletPosition, pelletDirection, _entityType, _config.MinigunProjectile));
        }
    }
}