using System;
using Abstracts;
using Gameplay.Abstracts;
using Gameplay.Mechanics.Timer;
using Gameplay.Shooting.Scriptables;
using Gameplay.Shooting.Weapons;
using Zenject;

namespace Gameplay.Shooting.Factories
{
    public sealed class WeaponFactory : IFactory<WeaponConfig, EntityType, Weapon>
    {
        private readonly ProjectileFactory _projectileFactory;
        private readonly TimerFactory _timerFactory;

        public WeaponFactory(ProjectileFactory projectileFactory, TimerFactory timerFactory)
        {
            _projectileFactory = projectileFactory;
            _timerFactory = timerFactory;
        }
        
        public Weapon Create(WeaponConfig weaponConfig, EntityType entityType)
        {
            return weaponConfig.Type switch
            {
                WeaponType.None => new NullGun(),
                WeaponType.Blaster => new Blaster(weaponConfig as BlasterConfig, entityType, _projectileFactory, _timerFactory),
                WeaponType.Shotgun => new Shotgun(weaponConfig as ShotgunConfig, entityType, _projectileFactory, _timerFactory),
                WeaponType.Minigun => new Minigun(weaponConfig as MinigunConfig, entityType, _projectileFactory, _timerFactory),
                WeaponType.Railgun => new Railgun(weaponConfig as RailgunConfig, entityType, _projectileFactory, _timerFactory),
                _ => throw new ArgumentOutOfRangeException(nameof(weaponConfig.Type), weaponConfig.Type,
                    "A not-existent weapon type is provided")
            };
        }
    }
}