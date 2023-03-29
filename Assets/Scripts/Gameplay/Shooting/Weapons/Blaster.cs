using Abstracts;
using Gameplay.Abstracts;
using Gameplay.Mechanics.Timer;
using Gameplay.Shooting.Factories;
using Gameplay.Shooting.Scriptables;
using UnityEngine;

namespace Gameplay.Shooting.Weapons
{
    public class Blaster : Weapon
    {
        private readonly BlasterConfig _blasterConfig;
        private readonly EntityType _entityType;
        private readonly ProjectileFactory _projectileFactory;

        public Blaster(BlasterConfig blasterConfig, EntityType entityType, ProjectileFactory projectileFactory, TimerFactory timerFactory)
        {
            _blasterConfig = blasterConfig;
            _entityType = entityType;
            _projectileFactory = projectileFactory;
            CooldownTimer = timerFactory.Create(blasterConfig.Cooldown);
        }
        
        public override void CommenceFiring(Vector2 bulletPosition, Quaternion turretDirection)
        {
            if (IsOnCooldown) return;

            _projectileFactory.Create(new ProjectileSpawnParams(bulletPosition, turretDirection, _entityType, _blasterConfig.BlasterProjectile));
            
            CooldownTimer.Start();
        }
    }
}