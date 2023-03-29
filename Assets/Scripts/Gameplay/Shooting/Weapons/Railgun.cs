using Abstracts;
using Gameplay.Abstracts;
using Gameplay.Mechanics.Timer;
using Gameplay.Shooting.Factories;
using Gameplay.Shooting.Scriptables;
using UnityEngine;

namespace Gameplay.Shooting.Weapons
{
    public class Railgun : Weapon
    {
        private readonly RailgunConfig _railgunConfig;
        private readonly EntityType _entityType;
        private readonly ProjectileFactory _projectileFactory;

        public Railgun(RailgunConfig railgunConfig, EntityType entityType, ProjectileFactory projectileFactory, TimerFactory timerFactory)
        {
            _railgunConfig = railgunConfig;
            _entityType = entityType;
            _projectileFactory = projectileFactory;
            CooldownTimer = timerFactory.Create(railgunConfig.Cooldown);
        }

        public override void CommenceFiring(Vector2 bulletPosition, Quaternion turretDirection)
        {
            if (IsOnCooldown) return;

            _projectileFactory.Create(new ProjectileSpawnParams(bulletPosition, turretDirection, _entityType, _railgunConfig.RailgunProjectile));
            
            CooldownTimer.Start();
        }
    }
}