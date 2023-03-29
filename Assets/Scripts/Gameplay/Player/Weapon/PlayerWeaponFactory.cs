using Abstracts;
using Gameplay.Abstracts;
using Gameplay.Input;
using Gameplay.Shooting;
using Gameplay.Shooting.Scriptables;
using Zenject;

namespace Gameplay.Player.Weapon
{
    public sealed class PlayerWeaponFactory : PlaceholderFactory<PlayerView, PlayerWeapon>
    {
        private readonly IFactory<MountedWeaponConfig, EntityView, MountedWeapon> _mountedWeaponFactory;
        private readonly MountedWeaponConfig _config;
        private readonly DiContainer _diContainer;

        public PlayerWeaponFactory(IFactory<MountedWeaponConfig, EntityView, MountedWeapon> mountedWeaponFactory, MountedWeaponConfig config, DiContainer diContainer)
        {
            _mountedWeaponFactory = mountedWeaponFactory;
            _config = config;
            _diContainer = diContainer;
        }

        public override PlayerWeapon Create(PlayerView playerView)
        {
            var playerInput = _diContainer.Resolve<PlayerInput>();
            return new PlayerWeapon(_mountedWeaponFactory.Create(_config, playerView), playerInput);
        }
    }
}