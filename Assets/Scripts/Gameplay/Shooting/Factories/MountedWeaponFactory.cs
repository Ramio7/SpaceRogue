using System;
using Abstracts;
using Gameplay.Abstracts;
using Gameplay.Shooting.Scriptables;
using Zenject;

namespace Gameplay.Shooting.Factories
{
    public class MountedWeaponFactory : IFactory<MountedWeaponConfig, EntityView, MountedWeapon>
    {
        private readonly IFactory<WeaponConfig, EntityType, Weapon> _weaponFactory;
        private readonly GunPointViewFactory _gunPointViewFactory;
        private readonly TurretViewFactory _turretViewFactory;

        public MountedWeaponFactory(IFactory<WeaponConfig, EntityType, Weapon> weaponFactory, GunPointViewFactory gunPointViewFactory, TurretViewFactory turretViewFactory)
        {
            _weaponFactory = weaponFactory;
            _gunPointViewFactory = gunPointViewFactory;
            _turretViewFactory = turretViewFactory;
        }

        public MountedWeapon Create(MountedWeaponConfig config, EntityView entityView)
        {
            return config.WeaponMountType switch
            {
                WeaponMountType.None => new UnmountedWeapon(CreateWeapon(config, entityView), entityView),
                WeaponMountType.Frontal => new FrontalMountedWeapon(CreateWeapon(config, entityView), entityView, _gunPointViewFactory),
                WeaponMountType.Turret => new TurretMountedWeapon(CreateWeapon(config, entityView), entityView, _turretViewFactory, _gunPointViewFactory),
                _ => throw new ArgumentOutOfRangeException(nameof(config), config, "A not-existent weapon mount type is provided")
            };
        }
        
        private Weapon CreateWeapon(MountedWeaponConfig config, EntityView entityView) => _weaponFactory.Create(config.MountedWeapon, entityView.EntityType);
    }
}