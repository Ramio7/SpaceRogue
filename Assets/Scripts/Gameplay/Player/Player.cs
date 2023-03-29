using System;
using Gameplay.Movement;
using Gameplay.Player.Weapon;
using Gameplay.Survival;

namespace Gameplay.Player
{
    public sealed class Player : IDisposable
    {
        private readonly UnitMovement _unitMovement;
        private readonly UnitTurningMouse _unitTurningMouse;
        private readonly PlayerWeapon _playerWeapon;

        public event Action PlayerDestroyed = () => { };

        public PlayerView PlayerView { get; }
        public EntitySurvival Survival { get; }

        public Player(
            PlayerView playerView, 
            UnitMovement unitMovement, 
            UnitTurningMouse unitTurningMouse,
            EntitySurvival playerSurvival,
            PlayerWeapon playerWeapon)
        {
            PlayerView = playerView;
            _unitMovement = unitMovement;
            _unitTurningMouse = unitTurningMouse;
            _playerWeapon = playerWeapon;
            Survival = playerSurvival;

            Survival.EntityHealth.HealthReachedZero += OnDeath;
        }

        public void Dispose()
        {
            Survival.EntityHealth.HealthReachedZero -= OnDeath;
            
            PlayerDestroyed.Invoke();
            
            Survival.Dispose();
            _unitMovement.Dispose();
            _unitTurningMouse.Dispose();
            _playerWeapon.Dispose();
            
            UnityEngine.Object.Destroy(PlayerView.gameObject);
        }

        private void OnDeath()
        {
            Dispose();
        }
    }
}