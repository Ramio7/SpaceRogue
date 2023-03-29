using Gameplay.Movement;
using Gameplay.Player;
using Services;
using System;
using UI.Game;
using UnityEngine;

namespace UI.Services
{
    public sealed class PlayerSpeedometerService : IDisposable
    {
        private readonly PlayerSpeedometerView _playerSpeedometerView;
        private readonly Updater _updater;
        private readonly PlayerMovementFactory _movementFactory;

        private UnitMovement _playerMovement;

        public PlayerSpeedometerService(Updater updater, PlayerInfoView playerInfoView, PlayerMovementFactory movementFactory)
        {
            _playerSpeedometerView = playerInfoView.PlayerSpeedometerView;
            _updater = updater;
            _movementFactory = movementFactory;

            _playerSpeedometerView.Hide();

            _movementFactory.PlayerMovementCreated += OnPlayerMovementCreated;
        }

        public void Dispose()
        {
            _movementFactory.PlayerMovementCreated -= OnPlayerMovementCreated;
            _updater.UnsubscribeFromUpdate(UpdateSpeedometer);
        }

        private void OnPlayerMovementCreated(UnitMovement playerMovement)
        {
            _playerMovement = playerMovement;

            _playerSpeedometerView.Show();
            _playerSpeedometerView.Init(GetSpeedometerTextValue(_playerMovement.CurrentSpeed, _playerMovement.MaxSpeed));

            _updater.SubscribeToUpdate(UpdateSpeedometer);
        }

        private void UpdateSpeedometer()
        {
            _playerSpeedometerView.UpdateText(GetSpeedometerTextValue(_playerMovement.CurrentSpeed, _playerMovement.MaxSpeed));
        }

        private string GetSpeedometerTextValue(float currentSpeed, float maximumSpeed)
        {
            return currentSpeed switch
            {
                < 0 => "R",
                _ => $"SPD: {Mathf.RoundToInt(currentSpeed / maximumSpeed * 100)}"
            };
        }
    }
}