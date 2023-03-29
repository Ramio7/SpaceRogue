using Abstracts;
using Gameplay.Abstracts;
using Gameplay.Mechanics.Timer;
using Gameplay.Shooting.Factories;
using Gameplay.Shooting.Scriptables;
using UnityEngine;
using Utilities.Mathematics;

namespace Gameplay.Shooting.Weapons
{
    public class Shotgun : Weapon
    {
        private readonly ShotgunConfig _config;
        private readonly EntityType _entityType;
        private readonly ProjectileFactory _projectileFactory;

        public Shotgun(ShotgunConfig config, EntityType entityType, ProjectileFactory projectileFactory, TimerFactory timerFactory)
        {
            _config = config;
            _entityType = entityType;
            _projectileFactory = projectileFactory;
            CooldownTimer = timerFactory.Create(config.Cooldown);
        }
        
        public override void CommenceFiring(Vector2 bulletPosition, Quaternion turretDirection)
        {
            if (IsOnCooldown) return;

            FireMultipleProjectiles(bulletPosition, turretDirection, _config.PelletCount, _config.SprayAngle);

            _projectileFactory.Create(new ProjectileSpawnParams(bulletPosition, turretDirection, _entityType, _config.ShotgunProjectile));
            
            CooldownTimer.Start();
        }

        private void FireMultipleProjectiles(Vector2 bulletPosition, Quaternion turretDirection, int count, float sprayAngle)
        {
            var minimumAngle = -sprayAngle / 2;
            var singlePelletAngle = sprayAngle / count;

            for (int i = 0; i < count; i++)
            {
                var minimumPelletAngle = minimumAngle + i * singlePelletAngle;
                var maximumPelletAngle = minimumAngle + (i + 1) * singlePelletAngle;

                var pelletAngle = RandomPicker.PickRandomBetweenTwoValues(minimumPelletAngle, maximumPelletAngle);
                Vector3 pelletVector = (pelletAngle + 90).ToVector3();
                Quaternion pelletDirection = turretDirection * Quaternion.Euler(pelletVector.x, pelletVector.y, pelletVector.z);
                
                _projectileFactory.Create(new ProjectileSpawnParams(bulletPosition, pelletDirection, _entityType, _config.ShotgunProjectile));
            }
        }
    }
}