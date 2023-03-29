using System;
using Abstracts;
using Gameplay.Abstracts;
using Services;
using UnityEngine;

namespace Gameplay.Input
{
    public sealed class PlayerInput : IDisposable, IUnitMovementInput, IUnitTurningMouseInput
    {
        private readonly Updater _updater;
        private readonly PlayerInputConfig _playerInputConfig;

        public event Action<Vector3> MousePositionInput = _ => { };
        
        public event Action<float> VerticalAxisInput = _ => { };
        public event Action<float> HorizontalAxisInput = _ => { };

        public event Action<bool> PrimaryFireInput = _ => { };
        public event Action<bool> ChangeWeaponInput = _ => { };
        public event Action<bool> NextLevelInput = _ => { };
        public event Action<bool> MapInput = _ => { };
        
        private const string Vertical = "Vertical";
        private const string Horizontal = "Horizontal";
        private const KeyCode PrimaryFire = KeyCode.Mouse0;
        private const KeyCode ChangeWeapon = KeyCode.Q;
        private const KeyCode NextLevel = KeyCode.Return;
        private const KeyCode Map = KeyCode.Tab;
        
        

        public PlayerInput(Updater updater, PlayerInputConfig playerInputConfig)
        {
            _updater = updater;
            _playerInputConfig = playerInputConfig;
            
            _updater.SubscribeToUpdate(CheckVerticalInput);
            _updater.SubscribeToUpdate(CheckHorizontalInput);
            _updater.SubscribeToUpdate(CheckFiringInput);
            _updater.SubscribeToUpdate(CheckMousePositionInput);
            _updater.SubscribeToUpdate(CheckChangeWeaponInput);
            _updater.SubscribeToUpdate(CheckNextLevelInput);
            _updater.SubscribeToUpdate(CheckMapInput);
        }

        public void Dispose()
        {
            _updater.UnsubscribeFromUpdate(CheckVerticalInput);
            _updater.UnsubscribeFromUpdate(CheckHorizontalInput);
            _updater.UnsubscribeFromUpdate(CheckFiringInput);
            _updater.UnsubscribeFromUpdate(CheckMousePositionInput);
            _updater.UnsubscribeFromUpdate(CheckChangeWeaponInput);
            _updater.UnsubscribeFromUpdate(CheckNextLevelInput);
            _updater.UnsubscribeFromUpdate(CheckMapInput);
        }
        
        private void CheckVerticalInput()
        {
            float verticalOffset = UnityEngine.Input.GetAxis(Vertical);
            float inputValue = CalculateInputValue(verticalOffset, _playerInputConfig.VerticalInputMultiplier);
            VerticalAxisInput(inputValue);
        }
        
        private void CheckHorizontalInput()
        {
            float horizontalOffset = UnityEngine.Input.GetAxis(Horizontal);
            float inputValue = CalculateInputValue(horizontalOffset, _playerInputConfig.HorizontalInputMultiplier);
            HorizontalAxisInput(inputValue);
        }

        private void CheckFiringInput()
        {
            bool value = UnityEngine.Input.GetKey(PrimaryFire);
            PrimaryFireInput(value);
        }

        private void CheckMousePositionInput()
        {
            Vector3 value = UnityEngine.Input.mousePosition;
            MousePositionInput(value);
        }

        private void CheckChangeWeaponInput()
        {
            bool value = UnityEngine.Input.GetKeyDown(ChangeWeapon);
            ChangeWeaponInput(value);
        }

        private void CheckNextLevelInput()
        {
            bool value = UnityEngine.Input.GetKeyDown(NextLevel);
            NextLevelInput(value);
        }
        
        private void CheckMapInput()
        {
            bool value = UnityEngine.Input.GetKey(Map);
            MapInput(value);
        }

        private static float CalculateInputValue(float axisOffset, float inputMultiplier)
            => axisOffset * inputMultiplier;
    }
}